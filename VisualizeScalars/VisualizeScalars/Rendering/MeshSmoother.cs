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
    public enum Smoothing { None, Laplacian1 = 1, Laplacian2 = 2, Laplacian5 = 5, Laplacian10 = 10, LaplacianHc1 = 1, LaplacianHc2 = 2, LaplacianHc5 = 5, LaplacianHc10 = 10 }
    public static class MeshSmoother
    {
        public static Mesh LaplacianFilter(Mesh model, int times = 1)
        {
            var ordered = model.Vertices.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            var newVerts =  LaplacianFilter(ordered, model.Indices.ToArray(), times);
            model.Vertices = new Dictionary<Vector3, int>();
            //model.Vertices = new List<Vector3>(newVerts);
            for (int i = 0; i < newVerts.Length; i++)
            {
                if (model.Vertices.ContainsKey(newVerts[i]))
                {
                    var index = model.Vertices[newVerts[i]];
                    int oldindex = -1;
                    while ((oldindex = model.Indices.IndexOf(i)) != -1)
                    {
                        model.Indices.RemoveAt(oldindex);
                        model.Indices.Insert(oldindex, index);
                        
                    }
                }
                else
                {
                    model.Vertices.Add(newVerts[i], i);
                }
            }
            return model;
        }

        public static Vector3[] LaplacianFilter(Vector3[] vertices, int[] triangles, int times)
        {
            var network = VertexConnection.BuildNetwork(triangles);
            for (int i = 0; i < times; i++)
            {
                vertices = LaplacianFilter(network, vertices, triangles);
            }

            return vertices;
        }

        static Vector3[] LaplacianFilter(Dictionary<int, VertexConnection> network, Vector3[] origin, int[] triangles)
        {
            Vector3[] vertices = new Vector3[origin.Length];
            for (int i = 0, n = origin.Length; i < n; i++)
            {
                var connection = network[i].Connection;
                var v = Vector3.Zero;
                foreach (int adj in connection)
                {
                    v += origin[adj];
                }

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
           // var vertices = HCFilter(mesh.Vertices.ToArray(), mesh.Indices.ToArray(), times, alpha, beta);
            var vertices = HCFilter(ordered, mesh.Indices.ToArray(), times, alpha, beta);
            mesh.Vertices = new Dictionary<Vector3, int>();
            //mesh.Vertices = new List<Vector3>(vertices);
            for (int i = 0; i < vertices.Length; i++)
            {
                mesh.Vertices.Add(vertices[i], i);
            }
            //mesh.RecalculateNormals();
            //mesh.RecalculateBounds();
            return mesh;
        }

        static Vector3[] HCFilter(Vector3[] vertices, int[] triangles, int times, float alpha, float beta)
        {
            alpha = Math.Clamp(alpha,0,1);
            beta = Math.Clamp(beta,0,1);

            var network = VertexConnection.BuildNetwork(triangles);

            Vector3[] origin = new Vector3[vertices.Length];
            Array.Copy(vertices, origin, vertices.Length);
            for (int i = 0; i < times; i++)
            {
                vertices = HCFilter(network, origin, vertices, triangles, alpha, beta);
            }

            return vertices;
        }

        public static Vector3[] HCFilter(Dictionary<int, VertexConnection> network, Vector3[] o, Vector3[] q,
            int[] triangles, float alpha, float beta)
        {
            Vector3[] p = LaplacianFilter(network, q, triangles);
            Vector3[] b = new Vector3[o.Length];

            for (int i = 0; i < p.Length; i++)
            {
                b[i] = p[i] - (alpha * o[i] + (1f - alpha) * q[i]);
            }

            for (int i = 0; i < p.Length; i++)
            {
                var adjacents = network[i].Connection;
                var bs = Vector3.Zero;
                foreach (int adj in adjacents)
                {
                    bs += b[adj];
                }

                p[i] = p[i] - (beta * b[i] + (1 - beta) / adjacents.Count * bs);
            }

            return p;
        }
    }

    public class VertexConnection
    {

        public HashSet<int> Connection
        {
            get { return connection; }
        }

        HashSet<int> connection;

        public VertexConnection()
        {
            this.connection = new HashSet<int>();
        }

        public void Connect(int to)
        {
            connection.Add(to);
        }

        public static Dictionary<int, VertexConnection> BuildNetwork(int[] triangles)
        {
            var table = new Dictionary<int, VertexConnection>();

            for (int i = 0, n = triangles.Length; i < n; i += 3)
            {
                int a = triangles[i], b = triangles[i + 1], c = triangles[i + 2];
                if (!table.ContainsKey(a))
                {
                    table.Add(a, new VertexConnection());
                }

                if (!table.ContainsKey(b))
                {
                    table.Add(b, new VertexConnection());
                }

                if (!table.ContainsKey(c))
                {
                    table.Add(c, new VertexConnection());
                }

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
	
