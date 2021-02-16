using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models
{
    public class Mesh
    {
        internal readonly List<Vector4> Colors = new List<Vector4>();
        internal Dictionary<int, Vector3> Normals = new Dictionary<int, Vector3>();
        public Mesh()
        {
        }

        public Mesh(Vector3[] vertices, int[] indices, Vector4[] colors)
        {
            for (var i = 0; i < vertices.Length; i++) Vertices.Add(vertices[i], i);
            Indices.AddRange(indices);
            Colors.AddRange(colors.Distinct());
            ColorIndices.AddRange(colors.Select(x => Colors.IndexOf(x)));
        }

        public Dictionary<Vector3, int> Vertices { get; set; } = new Dictionary<Vector3, int>();

        public List<int> ColorIndices { get; set; } = new List<int>();
        public List<int> Indices { get; set; } = new List<int>();

        public void AppendTriangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {

            Vertices.TryAdd(pos1, Vertices.Count);
            Vertices.TryAdd(pos2, Vertices.Count);
            Vertices.TryAdd(pos3, Vertices.Count);

            Indices.Add(Vertices[pos1]);
            Indices.Add(Vertices[pos2]);
            Indices.Add(Vertices[pos3]);
           
            var vertexNormal = MathHelpers.GetSurfaceNormal(pos1, pos2, pos3);
            

            if (!Normals.TryAdd(Vertices[pos1], vertexNormal))
            {
                Normals[Vertices[pos1]] += vertexNormal;
                Normals[Vertices[pos1]] = Normals[Vertices[pos1]].Normalized();
            }
            if (!Normals.TryAdd(Vertices[pos2], vertexNormal))
            {
                Normals[Vertices[pos2]] += vertexNormal;
                Normals[Vertices[pos2]] = Normals[Vertices[pos2]].Normalized();
            }
            if (!Normals.TryAdd(Vertices[pos3], vertexNormal))
            {
                Normals[Vertices[pos3]] += vertexNormal;
                Normals[Vertices[pos3]] = Normals[Vertices[pos3]].Normalized();
            }
        }

        public void AppendTriangle(Vector3[] pos)
        {
            AppendTriangle(pos[0], pos[1], pos[2]);
        }

        public void AppendTriangle(IVertex v1, IVertex v2, IVertex v3)
        {
            AppendTriangle(v1.Position,v2.Position,v3.Position);
            if (!Colors.Contains(v1.Color))
                Colors.Add(v1.Color);
            if (!Colors.Contains(v2.Color))
                Colors.Add(v2.Color);
            if (!Colors.Contains(v3.Color))
                Colors.Add(v3.Color);
            ColorIndices.Add(Colors.IndexOf(v1.Color));
            ColorIndices.Add(Colors.IndexOf(v2.Color));
            ColorIndices.Add(Colors.IndexOf(v3.Color));
        }

        public void AppendTriangle<T>(T[] vertices) where T : IVertex
        {
            var v1 = vertices[0];
            var v2 = vertices[1];
            var v3 = vertices[2];
            AppendTriangle(v1, v2, v3);
        }

        public T[] GetVertices<T>(bool avgerageNormals = true) where T : IVertex, new()
        {
            var positions = new List<T>();
            var ordered = Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();

            for (var i = 0; i < Indices.Count; i += 3)
            {
                var pos1 = ordered[Indices[i]];
                var pos2 = ordered[Indices[i + 1]];
                var pos3 = ordered[Indices[i + 2]];
                var defaultColor = new Vector4(0.5f, 0.5f, 0.5f, 1);
                var color1 = Colors.Count > 0 ? Colors[ColorIndices[i]] : defaultColor;
                var color2 = Colors.Count > 0 ? Colors[ColorIndices[i+1]] : defaultColor;
                var color3 = Colors.Count > 0 ? Colors[ColorIndices[i+2]] : defaultColor;
                /* 
                 var normal = MathHelpers.GetSurfaceNormal(pos1, pos2, pos3);
                 positions.Add(new T {Position = pos1, Normal = normal, Color = color});
                 positions.Add(new T {Position = pos2, Normal = normal, Color = color});
                 positions.Add(new T {Position = pos3, Normal = normal, Color = color});
                */
                if (Normals.Count == 0 || avgerageNormals == false)
                {
                    var normal = MathHelpers.GetSurfaceNormal(pos1, pos2, pos3);
                    positions.Add(new T { Position = pos1, Normal = normal, Color = color1 });
                    positions.Add(new T { Position = pos2, Normal = normal, Color = color2 });
                    positions.Add(new T { Position = pos3, Normal = normal, Color = color3 });
                }
                else
                {
                    var normal1 = Normals[Vertices[pos1]];
                    var normal2 = Normals[Vertices[pos2]];
                    var normal3 = Normals[Vertices[pos3]];
                    positions.Add(new T { Position = pos1, Normal = normal1, Color = color1 });
                    positions.Add(new T { Position = pos2, Normal = normal2, Color = color2 });
                    positions.Add(new T { Position = pos3, Normal = normal3, Color = color3 });
                }
            }

            return positions.ToArray();
        }

        public int[] GetIndices()
        {
            return Enumerable.Range(0, Indices.Count).ToArray();
        }
    }
}