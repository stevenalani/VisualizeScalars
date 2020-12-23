﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models
{
    public class Mesh
    {
        List<Vector4> Colors = new List<Vector4>();
        public Dictionary<Vector3, int> Vertices { get; set; } = new Dictionary<Vector3, int>();
        //public List<Vector3> PrimitiveNormals { get; set; } = new List<Vector3>();
        public List<int> ColorIndices { get; set; } = new List<int>();
        public List<int> Indices { get; set; } = new List<int>();

        public Mesh()
        {
            
        }
        public Mesh(Vector3[] vertices, int[] indices, Vector4[] colors)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vertices.Add(vertices[i],i);
            }
            Indices.AddRange(indices);
            Colors.AddRange(colors.Distinct());
            ColorIndices.AddRange(colors.Select(x => Colors.IndexOf(x)));
        }
        public void AppendTriangle(Vector3 pos1, Vector3 pos2, Vector3 pos3)
        {
            Vertices.TryAdd(pos1, Vertices.Count);
            Vertices.TryAdd(pos2, Vertices.Count);
            Vertices.TryAdd(pos3, Vertices.Count);
            Indices.Add(Vertices[pos1]);
            Indices.Add(Vertices[pos2]);
            Indices.Add(Vertices[pos3]);
        }

        public void AppendTriangle(Vector3[] pos)
        {
            AppendTriangle(pos[0], pos[1], pos[2]);
        }

        public void AppendTriangle(IVertex v1, IVertex v2, IVertex v3)
        {
            Vertices.TryAdd(v1.Position, Vertices.Count); 
            Vertices.TryAdd(v2.Position, Vertices.Count);
            Vertices.TryAdd(v3.Position, Vertices.Count);
            Indices.Add(Vertices[v1.Position]);
            Indices.Add(Vertices[v2.Position]);
            Indices.Add(Vertices[v3.Position]);
            if(!Colors.Contains(v1.Color))
                Colors.Add(v1.Color);
            if(!Colors.Contains(v2.Color))
                Colors.Add(v2.Color);
            if(!Colors.Contains(v3.Color))
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
        public T[] GetVertices<T>() where T:IVertex, new()
        {
            var positions = new List<T>();
            var ordered = Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            
            for (int i = 0; i < Indices.Count; i+=3)
            {
                
                var pos1 = ordered[Indices[i]];
                var pos2 = ordered[Indices[i +1]];
                var pos3 = ordered[Indices[i +2]];
                var normal = MathHelpers.GetSurfaceNormal(pos1, pos2, pos3);
                var color = Colors.Count > 0 ? Colors[ColorIndices[i]] : new Vector4(0.5f, 0.5f, 0.5f, 1);
                positions.Add(new T(){ Position = pos1, Normal = normal, Color = color });
                positions.Add(new T(){ Position = pos2, Normal = normal, Color = color });
                positions.Add(new T(){ Position = pos3, Normal = normal, Color = color });

            }

            return positions.ToArray();
        }

        public int[] GetIndices()
        {
            return Enumerable.Range(0, Indices.Count).ToArray();
        }


    }
    public class GridSurface : Mesh
    {
        public int Width { get; set; }
        public int Depth { get; set; }


        public GridSurface(float[,] heights,int width, int depth)
        {
            Depth = depth;
            Width = width;
            float min = Single.MaxValue;

            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    var value = heights[x, z];
                    if (value < min) min = value;
                }
            }

            for (int z = 0; z < Depth; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vertices.Add(new Vector3(x, heights[x, z] - min, z),Vertices.Count);
                }
            }
            for (int i = 0; i < Vertices.Count - width-1; i++)
            {
                if((i+1) % (width) == 0) 
                    continue;
                Indices.AddRange(new int[] {i, i + 1, i + 1 + width, i + 1 + width, i + width, i});
            }
        }

    }
}