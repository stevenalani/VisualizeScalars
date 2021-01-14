using System.Runtime.CompilerServices;
using OpenTK;

namespace VisualizeScalars.Rendering.Models
{
    public class GridSurface : Mesh
    {
        
        public GridSurface(float[,] heights, int width, int depth)
        {
            Depth = depth;
            Width = width;
            Heights = heights;
            var min = float.MaxValue;

            for (var z = 0; z < Depth; z++)
            for (var x = 0; x < Width; x++)
            {
                var value = heights[x, z];
                if (value < min) min = value;
            }

            for (var z = 0; z < Depth; z++)
            for (var x = 0; x < Width; x++)
                Vertices.Add(new Vector3(x, heights[x, z] - min, z), Vertices.Count);
            for (var i = 0; i < Vertices.Count - width - 1; i++)
            {
                if ((i + 1) % width == 0)
                    continue;
                Indices.AddRange(new[] {i, i + 1, i + 1 + width, i + 1 + width, i + width, i});
            }
        }

        private float[,] Heights;
        public int Width { get; set; }
        public int Depth { get; set; }
    }
}