using System.Linq;
using System.Runtime.CompilerServices;
using OpenTK;
using VisualizeScalars.Helpers;

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
            var ordered = Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            for (int i = 0; i < Indices.Count; i+=3)
            {
                var pos1 = ordered[Indices[i]];
                var pos2 = ordered[Indices[i+1]]; 
                var pos3 = ordered[Indices[i+2]];
                var vertexNormal = MathHelpers.GetSurfaceNormal(pos1, pos2, pos3);
                if (!Normals.TryAdd(Vertices[pos1], vertexNormal))
                {
                    Normals[Vertices[pos1]] += vertexNormal.Normalized();
                    Normals[Vertices[pos1]] = Normals[Vertices[pos1]].Normalized();
                }
                if (!Normals.TryAdd(Vertices[pos2], vertexNormal))
                {
                    Normals[Vertices[pos2]] += vertexNormal.Normalized();
                    Normals[Vertices[pos2]] = Normals[Vertices[pos2]].Normalized();
                }
                if (!Normals.TryAdd(Vertices[pos3], vertexNormal))
                {
                    Normals[Vertices[pos3]] += vertexNormal.Normalized();
                    Normals[Vertices[pos3]] = Normals[Vertices[pos3]].Normalized();
                }
            }
        }

        private float[,] Heights;
        public int Width { get; set; }
        public int Depth { get; set; }
    }
}