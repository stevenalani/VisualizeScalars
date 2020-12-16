using System.Collections.Generic;
using OpenTK;

namespace SoilSpot.Rendering.Models.Voxel
{
    public static class MeshUtils
    {
        // Finds a set of adjacent vertices for a given vertex
        // Note the success of this routine expects only the set of neighboring faces to eacn contain one vertex corresponding
        // to the vertex in question
        public static List<Vector3> findAdjacentNeighbors(Vector3[] v, uint[] t, Vector3 vertex)
        {
            var adjacentV = new List<Vector3>();
            var facemarker = new List<int>();
            var facecount = 0;

            // Find matching vertices
            for (var i = 0; i < v.Length; i++)
                if (vertex.X.Equals(v[i].X) &&
                    vertex.Y.Equals(v[i].Y) &&
                    vertex.Z.Equals(v[i].Z))
                {
                    uint v1 = 0;
                    uint v2 = 0;
                    var marker = false;

                    // Find vertex indices from the triangle array
                    for (var k = 0; k < t.Length; k = k + 3)
                        if (facemarker.Contains(k) == false)
                        {
                            v1 = 0;
                            v2 = 0;
                            marker = false;

                            if (i == t[k])
                            {
                                v1 = t[k + 1];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 1])
                            {
                                v1 = t[k];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 2])
                            {
                                v1 = t[k];
                                v2 = t[k + 1];
                                marker = true;
                            }

                            facecount++;
                            if (marker)
                            {
                                // Once face has been used mark it so it does not get used again
                                facemarker.Add(k);

                                // Add non duplicate vertices to the list
                                if (isVertexExist(adjacentV, v[v1]) == false)
                                    adjacentV.Add(v[v1]);
                                //Debug.Log("Adjacent vertex index = " + v1);

                                if (isVertexExist(adjacentV, v[v2]) == false)
                                    adjacentV.Add(v[v2]);
                                //Debug.Log("Adjacent vertex index = " + v2);
                                marker = false;
                            }
                        }
                }

            //Debug.Log("Faces Found = " + facecount);

            return adjacentV;
        }


        // Finds a set of adjacent vertices indexes for a given vertex
        // Note the success of this routine expects only the set of neighboring faces to eacn contain one vertex corresponding
        // to the vertex in question
        public static List<uint> findAdjacentNeighborIndexes(Vector3[] v, uint[] t, Vector3 vertex)
        {
            var adjacentIndexes = new List<uint>();
            var adjacentV = new List<Vector3>();
            var facemarker = new List<int>();
            var facecount = 0;

            // Find matching vertices
            for (var i = 0; i < v.Length; i++)
                if (vertex.X.Equals(v[i].X) &&
                    vertex.Y.Equals(v[i].Y) &&
                    vertex.Z.Equals(v[i].Z))
                {
                    uint v1 = 0;
                    uint v2 = 0;
                    var marker = false;

                    // Find vertex indices from the triangle array
                    for (var k = 0; k < t.Length; k = k + 3)
                        if (facemarker.Contains(k) == false)
                        {
                            v1 = 0;
                            v2 = 0;
                            marker = false;

                            if (i == t[k])
                            {
                                v1 = t[k + 1];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 1])
                            {
                                v1 = t[k];
                                v2 = t[k + 2];
                                marker = true;
                            }

                            if (i == t[k + 2])
                            {
                                v1 = t[k];
                                v2 = t[k + 1];
                                marker = true;
                            }

                            facecount++;
                            if (marker)
                            {
                                // Once face has been used mark it so it does not get used again
                                facemarker.Add(k);

                                // Add non duplicate vertices to the list
                                if (isVertexExist(adjacentV, v[v1]) == false)
                                {
                                    adjacentV.Add(v[v1]);
                                    adjacentIndexes.Add(v1);
                                    //Debug.Log("Adjacent vertex index = " + v1);
                                }

                                if (isVertexExist(adjacentV, v[v2]) == false)
                                {
                                    adjacentV.Add(v[v2]);
                                    adjacentIndexes.Add(v2);
                                    //Debug.Log("Adjacent vertex index = " + v2);
                                }

                                marker = false;
                            }
                        }
                }

            //Debug.Log("Faces Found = " + facecount);

            return adjacentIndexes;
        }

        // Does the vertex v exist in the list of vertices
        private static bool isVertexExist(List<Vector3> adjacentV, Vector3 v)
        {
            var marker = false;
            foreach (var vec in adjacentV)
                if (vec.X.Equals(v.X) &&
                    vec.Y.Equals(v.Y) &&
                    vec.Z.Equals(v.Z))
                {
                    marker = true;
                    break;
                }

            return marker;
        }
    }
}