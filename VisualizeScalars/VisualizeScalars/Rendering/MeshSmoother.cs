/*
 * @author mattatz / http://mattatz.github.io
 * https://www.researchgate.net/publication/220507688_Improved_Laplacian_Smoothing_of_Noisy_Surface_Meshes
 * http://graphics.stanford.edu/courses/cs468-12-spring/LectureSlides/06_smoothing.pdf
 * http://wiki.unity3d.com/index.php?title=MeshSmoother
 */

using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Rendering.Models;

namespace VisualizeScalars.Rendering
{
    public enum Smoothing
    {
        None,
        Laplacian1 = 1,
        Laplacian2 = 2,
        Laplacian5 = 5,
        Laplacian10 = 10,
        LaplacianHc1 = 1,
        LaplacianHc2 = 2,
        LaplacianHc5 = 5,
        LaplacianHc10 = 10,
        RecalculateNormals
    }

    public static class MeshSmoother
    {
        public static Mesh LaplacianFilter(Mesh mesh, int times = 1)
        {
            var ordered = mesh.Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            var newVerts = LaplacianFilter(ordered, mesh.Indices.ToArray(), times);
            mesh.Vertices = new Dictionary<Vector3, int>();
            for (var i = 0; i < newVerts.Length; i++)
                if (mesh.Vertices.ContainsKey(newVerts[i]))
                {
                    var index = mesh.Vertices[newVerts[i]];
                    var oldindex = -1;
                    while ((oldindex = mesh.Indices.IndexOf(i)) != -1)
                    {
                        mesh.Indices.RemoveAt(oldindex);
                        mesh.Indices.Insert(oldindex, index);
                    }
                }
                else
                {
                    mesh.Vertices.Add(newVerts[i], i);
                }

            return mesh;
        }

        public static Vector3[] LaplacianFilter(Vector3[] vertices, int[] triangles, int times)
        {
            var network = VertexConnection.BuildNetwork(triangles);
            for (var i = 0; i < times; i++) vertices = LaplacianFilter(network, vertices, triangles);

            return vertices;
        }

        private static Vector3[] LaplacianFilter(Dictionary<int, VertexConnection> network, Vector3[] origin,
            int[] triangles)
        {
            var vertices = new Vector3[origin.Length];
            for (int i = 0, n = origin.Length; i < n; i++)
            {
                var connection = network[i].Connection;
                var v = Vector3.Zero;
                foreach (var adj in connection) v += origin[adj];

                vertices[i] = v / connection.Count;
            }

            return vertices;
        }

        /*
         * HC (Humphrey’s Classes) Smooth Algorithm - Reduces Shrinkage of Laplacian Smoother
         * alpha 0.0 ~ 1.0
         * beta  0.0 ~ 1.0
        */
        public static Mesh HCFilter(Mesh mesh, int times = 5, float alpha = 0.5f, float beta = 0.75f)
        {
            var ordered = mesh.Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            var vertices = HCFilter(ordered, mesh.Indices.ToArray(), times, alpha, beta);
            mesh.Vertices = new Dictionary<Vector3, int>();
            for (var i = 0; i < vertices.Length; i++) mesh.Vertices.Add(vertices[i], i);
            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
            return mesh;
        }

        private static Vector3[] HCFilter(Vector3[] vertices, int[] triangles, int times, float alpha, float beta)
        {
            alpha = Math.Clamp(alpha, 0, 1);
            beta = Math.Clamp(beta, 0, 1);

            var network = VertexConnection.BuildNetwork(triangles);

            var origin = new Vector3[vertices.Length];
            Array.Copy(vertices, origin, vertices.Length);
            for (var i = 0; i < times; i++) vertices = HCFilter(network, origin, vertices, triangles, alpha, beta);

            return vertices;
        }

        public static Vector3[] HCFilter(Dictionary<int, VertexConnection> network, Vector3[] o, Vector3[] q,
            int[] triangles, float alpha, float beta)
        {
            var p = LaplacianFilter(network, q, triangles);
            var b = new Vector3[o.Length];

            for (var i = 0; i < p.Length; i++) b[i] = p[i] - (alpha * o[i] + (1f - alpha) * q[i]);

            for (var i = 0; i < p.Length; i++)
            {
                var adjacents = network[i].Connection;
                var bs = Vector3.Zero;
                foreach (var adj in adjacents) bs += b[adj];

                p[i] = p[i] - (beta * b[i] + (1 - beta) / adjacents.Count * bs);
            }

            return p;
        }

    }


    public class VertexConnection
    {
        public VertexConnection()
        {
            Connection = new HashSet<int>();
        }

        public HashSet<int> Connection { get; }

        public void Connect(int to)
        {
            Connection.Add(to);
        }

        public static Dictionary<int, VertexConnection> BuildNetwork(int[] triangles)
        {
            var table = new Dictionary<int, VertexConnection>();

            for (int i = 0, n = triangles.Length; i < n; i += 3)
            {
                int a = triangles[i], b = triangles[i + 1], c = triangles[i + 2];
                if (!table.ContainsKey(a)) table.Add(a, new VertexConnection());

                if (!table.ContainsKey(b)) table.Add(b, new VertexConnection());

                if (!table.ContainsKey(c)) table.Add(c, new VertexConnection());

                table[a].Connect(b);
                table[a].Connect(c);
                table[b].Connect(a);
                table[b].Connect(c);
                table[c].Connect(a);
                table[c].Connect(b);
            }

            return table;
        }
    }
}