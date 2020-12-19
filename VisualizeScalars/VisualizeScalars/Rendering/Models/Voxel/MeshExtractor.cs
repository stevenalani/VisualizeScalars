using System;
using OpenTK;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public enum MeshMode { MarchingCubes, Cubes, GreedyCubes }
    public static class MeshExtractor
    {
        public static Mesh ComputeMarchingCubesMesh<T>(Volume<T> volume,Func<T,float> densityFunc,float isolevel = 0.1f) where T : IVolumeData , new()
        {
            var results = MarchingCubes.run<T>(volume, densityFunc, isolevel);
            Mesh mesh = new Mesh();
            foreach (var result in results)
            {
                mesh.AppendTriangle(result.p);
            }

            return mesh;
        }
        public static Mesh ComputeCubicMesh<T>(Volume<T> volume) where T : IVolumeData, new()
        {
            Mesh mesh = new Mesh();
            for (var currentZ = 0; currentZ < volume.Dimensions.Z; currentZ++)
            {
                for (var currentY = 0; currentY < volume.Dimensions.Y; currentY++)
                {
                    for (var currentX = 0; currentX < volume.Dimensions.X; currentX++)
                    {
                        var dataIndex = volume.DataPointers[currentY][currentX, currentZ];
                        if (dataIndex == 0) continue;

                        Vector4 color = volume.Data[dataIndex].ColorMapping;

                        var offset = new Vector3(currentX, currentY, currentZ);
                        if (volume.IsFrontfaceVisible(currentX, currentY, currentZ))
                        {
                            var width = volume.GetSameNeighborsX(currentX, currentY, currentZ, volume.IsFrontfaceVisible);
                            var height = volume.GetSameNeighborsY(currentX, currentY, currentZ, volume.IsFrontfaceVisible);
                            var depth = volume.GetSameNeighborsZ(currentX, currentY, currentZ, volume.IsFrontfaceVisible);
                            var test = CubeWithNormals.FrontFace(offset, color, width, height, depth, volume.CubeScale);
                            var verts = CubeWithNormals.FrontFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (volume.IsBackfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.BackFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (volume.IsLeftfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.LeftFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (volume.IsRightfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.RightFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (volume.IsBottomfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.BottomFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (volume.IsTopfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.TopFace(offset, color, volume.CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                    }
                }
            }

            return mesh;
        }
        public static Mesh ComputeCubicMeshGreedy<T>(Volume<T> volume) where T : IVolumeData, new()
        {
            Mesh mesh = new Mesh();
            int countX, countY, countZ;
            var _checked = 0;
            bool[][,] CheckedInVoxels = new bool[volume.Dimensions.Y][,];
            for (int i = 0; i < volume.Dimensions.Y; i++)
            {
                CheckedInVoxels[i] = new bool[volume.Dimensions.X, volume.Dimensions.Z];
            }
            for (var currentZ = 0; currentZ < volume.Dimensions.Z; currentZ++)
            {
                for (var currentY = 0; currentY < volume.Dimensions.Y; currentY++)
                {
                    for (var currentX = 0; currentX < volume.Dimensions.X; currentX++)
                    {
                        Vector3[] cube = new Vector3[8];
                        var currentvoxel = volume.DataPointers[currentY][currentX, currentZ];
                        if (currentvoxel == 0 || CheckedInVoxels[currentY][currentX, currentZ])
                            continue;

                        _checked++;
                        countX = volume.GetSameNeighborsX(currentX, currentY, currentZ,(i, i1, arg3) => true );
                        countY = volume.GetSameNeighborsY(currentX, currentY, currentZ, (i, i1, arg3) => true);
                        countZ = volume.GetSameNeighborsZ(currentX, currentY, currentZ, (i, i1, arg3) => true);
                        if (countX >= countY && countX >= countZ)
                            for (var i = (int)currentX; i <= currentX + countX; i++)
                            {
                                var voxelsAbove = volume.GetSameNeighborsY(i, currentY, currentZ, (i, i1, arg3) => true);
                                var voxelsInfront = volume.GetSameNeighborsZ(i, currentY, currentZ, (i, i1, arg3) => true);
                                if (voxelsAbove < countY || countY == -1)
                                    countY = voxelsAbove;
                                if (voxelsInfront < countZ || countZ == -1)
                                    countZ = voxelsInfront;
                            }
                        else if (countY >= countX && countY >= countZ)
                            for (var i = currentY; i <= currentY + countY; i++)
                            {
                                var voxelsRight = volume.GetSameNeighborsX(currentX, i, currentZ, (i, i1, arg3) => true);
                                var voxelsInfront = volume.GetSameNeighborsZ(currentX, i, currentZ, (i, i1, arg3) => true);
                                if (voxelsRight < countX || countX == -1)
                                    countX = voxelsRight;
                                if (voxelsInfront < countZ || countZ == -1)
                                    countZ = voxelsInfront;
                            }
                        else if (countZ >= countX && countZ >= countY)
                            for (var i = (int)currentZ; i <= currentZ + countZ; i++)
                            {

                                var voxelsAbove = volume.GetSameNeighborsY(currentX, currentY, i, (i, i1, arg3) => true);
                                var voxelsRight = volume.GetSameNeighborsX(currentX, currentY, i, (i, i1, arg3) => true);
                                if (voxelsAbove < countY || countY == -1)
                                    countY = voxelsAbove;
                                if (voxelsRight < countX || countX == -1)
                                    countX = voxelsRight;
                            }

                        var posxColorVertex = new Vector3(currentX, currentY, currentZ);
                        posxColorVertex.Z = currentZ + countZ + 1;
                        cube[0] = posxColorVertex;
                        posxColorVertex.X = currentX + countX + 1;
                        cube[1] = posxColorVertex;
                        posxColorVertex.Y = currentY + countY + 1;
                        cube[2] = posxColorVertex;
                        posxColorVertex.X = currentX;
                        cube[3] = posxColorVertex;

                        //Backface Vertex
                        posxColorVertex.X = currentX;
                        posxColorVertex.Y = currentY;
                        posxColorVertex.Z = currentZ;
                        cube[4] = posxColorVertex;

                        posxColorVertex.X = currentX + countX + 1;
                        cube[5] = posxColorVertex;
                        posxColorVertex.Y = currentY + countY + 1;
                        cube[6] = posxColorVertex;
                        posxColorVertex.X = currentX;
                        cube[7] = posxColorVertex;
                        var end = new Vector3Int(currentX + countX + 1, currentY + countY + 1,
                            currentZ + countZ + 1);
                        CheckIn<T>(ref volume,ref CheckedInVoxels, new Vector3Int(currentX, currentY, currentZ), end);
                        mesh.AppendTriangle(cube[0], cube[1], cube[2]);
                        mesh.AppendTriangle(cube[2], cube[3], cube[0]);

                        mesh.AppendTriangle(cube[1], cube[5], cube[6]);
                        mesh.AppendTriangle(cube[6], cube[2], cube[1]);

                        mesh.AppendTriangle(cube[4], cube[5], cube[6]);
                        mesh.AppendTriangle(cube[6], cube[7], cube[4]);

                        mesh.AppendTriangle(cube[0], cube[4], cube[7]);
                        mesh.AppendTriangle(cube[7], cube[3], cube[0]);

                        mesh.AppendTriangle(cube[3], cube[7], cube[6]);
                        mesh.AppendTriangle(cube[6], cube[2], cube[3]);

                        mesh.AppendTriangle(cube[0], cube[1], cube[5]);
                        mesh.AppendTriangle(cube[5], cube[4], cube[0]);
                    }
                }
            }

            return mesh;
        }
        private static void CheckIn<T>(ref Volume<T> volume,ref bool[][,] checkedin, Vector3Int start, Vector3Int end) where T : IVolumeData, new()
        {
            for (var i = start.X; i < end.X; i++)
            for (var j = start.Y; j < end.Y; j++)
            for (var k = start.Z; k < end.Z; k++)
            {
                var voxel = volume.DataPointers[j][i, k];
                if (volume.Data[voxel].IsSetVoxel())
                    checkedin[j][i, k] = true;
            }
        }

        private  static void CheckOut<T>(ref bool[][,] checkedin, Vector3Int start, Vector3Int end)
        {
            for (var x = start.X; x < end.X; x++)
            for (var y = start.Y; y < end.Y; y++)
            for (var z = start.Z; z < end.Z; z++)
                checkedin[y][x, z] = false;
        }

    }
}