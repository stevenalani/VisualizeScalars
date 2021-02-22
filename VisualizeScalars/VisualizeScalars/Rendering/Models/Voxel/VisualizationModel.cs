using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using OpenTK;
using VisualizeScalars.DataQuery;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public class VisualizationModel<T> : ColorVolume<T> where T : BaseGridCell, new()
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
            Dimensions.X = colCount;
            Dimensions.Y = (int) Math.Ceiling(deltaheight);
            Dimensions.Z = rowCount;
            InitializeVolumeData();


            for (var z = 0; z < rowCount; z++)
            for (var x = 0; x < colCount; x++)
            {
                var minNeighbour = getNeighbours(ref height, x, z).Min();
                var value = Math.Ceiling(height[x, z]);
                for (var i = minNeighbour - minVal -1; i <= value - minVal; i++)
                    SetVoxel(x , (int) (i), z, DataGrid[x, z]);
            }
        }

        public Bitmap[] GetScalarImages(Vector3[] colors, float radius)
        {
            foreach (var propertyName in DataGrid.PropertyNames)
            {
                if (propertyName ==HeightMapping ) continue;
                var grid = DataGrid.GetDataGrid(propertyName, true);


            }

            return null;
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