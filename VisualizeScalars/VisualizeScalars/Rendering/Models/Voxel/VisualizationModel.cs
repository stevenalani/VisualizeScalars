using System;
using System.Collections.Generic;
using System.Linq;
using VisualizeScalars.DataQuery;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    internal class VisualizationModel<T> : ColorVolume<T> where T : BaseGridCell, new()
    {
        public delegate float DensityFunction();

        private int colCount;
        private int rowCount;
        private int TextureID;

        public VisualizationModel(DataGrid<T> data) : base(1, 1, 1)
        {
            DataGrid = data;
        }

        public DataGrid<T> DataGrid { get; set; }
        public byte[] ImageBuffer { get; set; }

        public string HeightMapping { get; set; }

        public void GenerateVolume(string HeightScalar = "Height")
        {
            HeightMapping = HeightScalar;
            var height = DataGrid.GetDataGrid(HeightScalar);
            colCount = DataGrid.Width;
            rowCount = DataGrid.Height;
            var maxVal = height[0, 0];
            var minVal = height[0, 0];
            for (var z = 1; z < rowCount; z++)
            for (var x = 1; x < colCount; x++)
            {
                var value = height[x, z];
                if (maxVal < value) maxVal = value;
                if (minVal > value) minVal = value;
            }

            var deltaheight = maxVal - minVal;
            Dimensions.X = colCount + 2;
            Dimensions.Y = (int) Math.Ceiling(deltaheight + 4);
            Dimensions.Z = rowCount + 2;
            InitializeVolumeData();


            for (var z = 0; z < rowCount; z++)
            for (var x = 0; x < colCount; x++)
            {
                var minNeighbour = getNeighbours(ref height, x, z).Min();
                var value = Math.Ceiling(height[x, z]);
                for (var i = minNeighbour - minVal - 2; i <= value - minVal; i++)
                    SetVoxel(x + 1, (int) (i + 1), z + 1, DataGrid[x, z]);
            }
        }

        public BufferStorage[] GetBuffers()
        {
            var data = new Dictionary<string, List<float>>();
            var keys = DataGrid.PropertyNames;
            foreach (var propertyName in keys)
            {
                if (propertyName == HeightMapping) continue;
                data.Add(propertyName, new List<float>());
            }

            for (var i = 0; i < DataGrid.Height; i++)
            for (var j = 0; j < DataGrid.Width; j++)
                foreach (var propertyName in data.Keys)
                {
                    if (propertyName == HeightMapping) continue;
                    var value = DataGrid.ValueNormalized(j, i, propertyName);
                    if (value > 0)
                    {
                        data[propertyName].Add(j);
                        data[propertyName].Add(i);
                        data[propertyName].Add(value);
                    }
                }

            return data.Values.Select(x => new BufferStorage(x.ToArray())).ToArray();
        }

        /*
        public override void InitBuffers()
        {
            if (MeshMode == MeshMode.MarchingCubes)
            {
                ComputeMarchingCubesMesh();
            }
            else if (MeshMode == MeshMode.Cubes)
            {
                ComputeCubicMesh();
            }
            else if (MeshMode == MeshMode.GreedyCubes)
            {
                ComputeCubicMeshGreedy();
            }
            if (this.Smoothing == Smoothing.Laplacian1 ||
                this.Smoothing == Smoothing.Laplacian2 ||
                this.Smoothing == Smoothing.Laplacian5 ||
                this.Smoothing == Smoothing.Laplacian10 ||
                this.Smoothing == Smoothing.LaplacianHc1 ||
                this.Smoothing == Smoothing.LaplacianHc2 ||
                this.Smoothing == Smoothing.LaplacianHc5 ||
                this.Smoothing == Smoothing.LaplacianHc10)
            {
                if (this.Smoothing == Smoothing.Laplacian1 ||
                    this.Smoothing == Smoothing.Laplacian2 ||
                    this.Smoothing == Smoothing.Laplacian5 ||
                    this.Smoothing == Smoothing.Laplacian10)
                {
                    mesh = MeshSmoother.LaplacianFilter(mesh, (int)this.Smoothing);
                }
                else
                {
                    mesh = MeshSmoother.HCFilter(mesh, (int)this.Smoothing);
                }
            }
            mesh = MeshExtractor.ComputeMesh(this);
            //Buffers.ForEach(b => b.Dispose());
            //Buffers = new List<BufferStorage>();
            var linear = Data.Grid.Cast<GridCell>();
            List<float> data1 = new List<float>();
            List<float> data2 = new List<float>();
            List<float> data3 = new List<float>();
            for (int i = 0; i < DataGrid.Height; i++)
            {
                for (int j = 0; j < DataGrid.Width; j++)
                {
                    var value = (float)DataGrid[j, i].Humidity;
                    if (value > 0)
                    {
                        data1.Add((float) j);
                        data1.Add((float) i);
                        data1.Add(value);
                    }
                    value = (float)DataGrid[j, i].ParticulateMatter10;
                    if (value > 0)
                    {
                        data2.Add((float) j);
                        data2.Add((float) i);
                        data2.Add(value);
                    }
                    value = (float)DataGrid[j, i].ParticulateMatter2_5;
                    if (value > 0)
                    {
                        data3.Add((float) j);
                        data3.Add((float) i);
                        data3.Add(value);
                    }
                }
            }
            
            Buffers.Add(new BufferStorage(data1.ToArray()));
            Buffers.Add(new BufferStorage(data2.ToArray()));
            Buffers.Add(new BufferStorage(data3.ToArray()));

            var stream = File.OpenWrite("d:\\data.dat");
            BinaryWriter writer = new BinaryWriter(stream);
            writer.Write(data1.SelectMany(x => BitConverter.GetBytes(x)).ToArray());
            writer.Close();
            stream.Close();
            stream = File.OpenWrite("d:\\data2.dat");
            writer = new BinaryWriter(stream);
            writer.Write(data2.SelectMany(x => BitConverter.GetBytes(x)).ToArray());
            writer.Close();
            stream.Close();
            stream = File.OpenWrite("d:\\data3.dat");
            writer = new BinaryWriter(stream);
            writer.Write(data3.SelectMany(x => BitConverter.GetBytes(x)).ToArray());
            writer.Close();
            stream.Close();
            
            base.InitBuffers();
            
        }

        public override void Draw(ShaderProgram shader)
        {
            if (!IsReady) InitBuffers();
            shader.SetUniformVector4("LayerColor[0]", new Vector4(0.5f, 0.5f, 0.5f,1f));
            shader.SetUniformVector4("LayerColor[1]", new Vector4(1f, 0f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[2]", new Vector4(0f, 1f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[3]", new Vector4(0f, 0f, 1f,.3f));
            shader.SetUniformVector4("LayerColor[4]", new Vector4(1f, 0f, 0f,.3f));
            shader.SetUniformVector4("LayerColor[5]", new Vector4(0f, 1f, 0f,.3f));
            var idx = 0;
            foreach (var buffer in Buffers)
            {
                shader.SetUniformFloat($"BufferCnt[{idx++}]", buffer.ValueCount);
                buffer.Activate();
            }
            GL.BindVertexArray(Vao);
            GL.DrawElementsInstanced(PrimitiveType.Triangles,Indices.Length,DrawElementsType.UnsignedInt,IntPtr.Zero,  4);
            GL.BindVertexArray(0);
        }*/

        private float[] getNeighbours(ref float[,] heights, int x, int z)
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