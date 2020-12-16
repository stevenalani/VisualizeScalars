using System;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp.PixelFormats;
using VisualizeScalars.DataQuery;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;
using Vector4 = OpenTK.Vector4;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    class VisualizationModel : ColorVolume
    {
        public int Ssbo = -1;
        public float[] ssboData;

        public VisualizationDataGrid VisualizationData { get; private set; }
        private int colCount = 0;
        private int rowCount = 0;

        public VisualizationModel(VisualizationDataGrid visualizationData, float scale = 1f) : base(1,1,1,1)
        {
            this.VisualizationData = visualizationData;
            GenerateVolume("Height", scale);
        }

        public void GenerateVolume(string HeightScalar = "Height",float scale = 1f)
        {
            if (scale > 1)
            {
                VisualizationData.Scale(scale,false);
            }

            float[,] height = (float[,])this.VisualizationData.GetType().GetMethod($"{HeightScalar}Grid").Invoke(VisualizationData,new object[]{ false });
            colCount = VisualizationData.Width;
            rowCount = VisualizationData.Height;
            float maxVal = height[0, 0];
            float minVal = height[0, 0];
            var mat = new Material() { Color = new Vector4(0.5f, 0.5f, 0.5f, 1f) };
            for (var z = 1; z < rowCount; z++)
                for (var x = 1; x < colCount; x++)
                {
                    var value = height[x, z];
                    if (maxVal < value) maxVal = value;
                    if (minVal > value) minVal = value;
                }
            var deltaheight = maxVal - minVal;
            this.Dimensions.X = colCount + 2;
            this.Dimensions.Y = (int) Math.Ceiling(deltaheight + 4);
            this.Dimensions.Z = rowCount + 2;
            this.InitializeVolumeData();


            for (var z = 0; z < rowCount; z++)
                for (var x = 0; x < colCount; x++)
                {
                    var minNeighbour = getNeighbours(ref height, x, z).Min();
                    var value = Math.Ceiling(height[x, z]);
                    for (var i = minNeighbour - minVal - 1; i <= value - minVal; i++)
                    {
                        SetVoxel(x + 1, (int)(i + 1), z + 1, mat);
                    }
                }

            Position = new OpenTK.Vector3(Dimensions.X / -2f, -minVal, Dimensions.Z / -2f);
        }
        private byte[] generateTex()
        {
            var totalLength = VisualizationData.Grid.Length;
            var lengthX = VisualizationData.Width;
            var lengthY = VisualizationData.Height;
            byte[] data = new byte[totalLength * 5];
            int datapos = 0;
            var linearScalarSet = VisualizationData.Grid.Cast<ScalarSet>();
            var maxTemp = linearScalarSet.Max(x => x.Temperature);
            var minTemp = linearScalarSet.Min(x => x.Temperature);
            var maxPress = linearScalarSet.Max(x => x.Pressure);
            var minPress = linearScalarSet.Min(x => x.Pressure);
            var maxHum = linearScalarSet.Max(x => x.Humiditiy);
            var minHum = linearScalarSet.Min(x => x.Humiditiy);
            var maxPm10 = linearScalarSet.Max(x => x.ParticulateMatter10);
            var minPm10 = linearScalarSet.Min(x => x.ParticulateMatter10);
            var maxPm2 = linearScalarSet.Max(x => x.ParticulateMatter2_5);
            var minPm2 = linearScalarSet.Min(x => x.ParticulateMatter2_5);
            maxTemp = double.IsNaN(maxTemp) ? 0.0 : maxTemp;
            minTemp = double.IsNaN(minTemp) ? 0.0 : minTemp;
            maxPress = double.IsNaN(maxPress) ? 0.0 : maxPress;
            minPress = double.IsNaN(minPress) ? 0.0 : minPress;
            maxHum = double.IsNaN(maxHum) ? 0.0 : maxHum;
            minHum = double.IsNaN(minHum) ? 0.0 : minHum;
            maxPm10 = double.IsNaN(maxPm10) ? 0.0 : maxPm10;
            minPm10 = double.IsNaN(minPm10) ? 0.0 : minPm10;
            maxPm2 = double.IsNaN(maxPm2) ? 0.0 : maxPm2;
            minPm2 = double.IsNaN(minPm2) ? 0.0 : minPm2;
            for (int y = 0; y < lengthY; y++)
            {
                for (int x = 0; x < lengthX; x++)
                {
                    // Pass R16 texture and assign colors by users choice!! -> better approach
                    
                    data[datapos] = new A8((float)NormalizeValue(VisualizationData[x, y].Temperature,minTemp, maxTemp)).PackedValue;
                    data[datapos + totalLength] = new A8((float)NormalizeValue(VisualizationData[x, y].Pressure, minPress, maxPress)).PackedValue;
                    data[datapos + totalLength * 2] =
                        new A8((float) NormalizeValue(VisualizationData[x, y].Humiditiy, minHum, maxHum)).PackedValue;
                    data[datapos + totalLength * 3] =
                        new A8((float) NormalizeValue(VisualizationData[x, y].ParticulateMatter10, minPm10, maxPm10))
                            .PackedValue;
                    data[datapos + totalLength * 4] =
                        new A8((float) NormalizeValue(VisualizationData[x, y].ParticulateMatter2_5, minPm2, maxPm2))
                            .PackedValue;
                    //data[datapos + 1 + totalLength * 3] = color[1];
                    /*var color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].BulkDensity / VisualizationDataGrid.MaxBulkDensity)).PackedValue);
                    data[datapos] = color[0];
                    data[datapos + 1] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].FieldCapacity / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength] = color[0];
                    data[datapos + 1 + totalLength] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].ProfileAvailableWaterCapacity / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength * 2] = color[0];
                    data[datapos + 1 + totalLength * 2] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].SoilCarbonDensity / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength * 3] = color[0];
                    data[datapos + 1 + totalLength * 3] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].ThermalCapacity / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength * 4] = color[0];
                    data[datapos + 1 + totalLength * 4] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].TotalNitrogenDensity / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength * 5] = color[0];
                    data[datapos + 1 + totalLength * 5] = color[1];
                    color = BitConverter.GetBytes(new HalfSingle((float)(VisualizationData[x, y].WiltingPoint / VisualizationDataGrid.MaxFieldCapacity)).PackedValue);
                    data[datapos + totalLength * 6] = color[0];
                    data[datapos + 1 + totalLength * 6] = color[1];*/
                    datapos++;
                }
            }
            return data;
        }
        
        public override void Draw(ShaderProgram shader)
        {
            if (!IsReady) InitBuffers();
            shader.SetUniformMatrix4X4("model", Modelmatrix);
            shader.SetUniformVector4("LayerColor[0]", new Vector4(0.5f, 0.5f, 0.5f,1f));
            shader.SetUniformVector4("LayerColor[1]", new Vector4(1f, 0f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[2]", new Vector4(0f, 1f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[3]", new Vector4(0f, 0f, 1f,.3f));
            shader.SetUniformVector4("LayerColor[4]", new Vector4(1f, 0f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[5]", new Vector4(0f, 1f, 0f,.3f));
            /*GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture3D, TextureID);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, Ssbo);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, new IntPtr(ssboData.Length * sizeof(float)), ssboData, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, Ssbo);
         
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);   */
            GL.BindVertexArray(Vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles,Indices.Length,DrawElementsType.UnsignedInt,IntPtr.Zero,  6);
            GL.BindVertexArray(0);
        }

        

        private static double NormalizeValue(double val, double min, double max)
        {
            if (val == 0.0 || double.IsNaN(val) || max == 0.0) 
                return 0.0;
            return (val - min) / (max - min);
        }
        private static double NormalizeValue(double val, double max)
        {
            return NormalizeValue(val, 0, max);
        }
        private float[] getNeighbours(ref float[,] heights,int x, int z)
        {
            float[] neighbours;
            if (x == 0 && z == 0)
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x + 1, z + 1],
                        heights[x + 1, z]
                };
            else if (x == 0 && z < rowCount - 1)
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x + 1, z + 1],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x == 0 && z == rowCount - 1)
                neighbours = new[]
                {
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x < colCount - 1 && z == 0)
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x + 1, z + 1],
                        heights[x - 1, z],
                        heights[x + 1, z]
                };
            else if (x < colCount - 1 && z == rowCount - 1)
                neighbours = new[]
                {
                        heights[x - 1, z],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x == colCount - 1 && z == 0)
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x - 1, z]
                };
            else if (x == colCount - 1 && z < rowCount - 1)
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x - 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1]
                };
            else if (x == colCount - 1 && z == rowCount - 1)
                neighbours = new[]
                {
                        heights[x - 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1]
                };
            else
                neighbours = new[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x + 1, z + 1],
                        heights[x - 1, z],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1],
                        heights[x + 1, z - 1]
                };

            return neighbours;
        }

    }
}
