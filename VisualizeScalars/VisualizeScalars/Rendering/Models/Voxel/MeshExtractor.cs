using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using OpenTK;
using VisualizeScalars.DataQuery;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public enum MeshMode
    {
        MarchingCubes,
        Cubes,
        GreedyCubes,
        GridMesh
    }

    public static class MeshExtractor
    {
        public static Mesh ComputeMarchingCubesMesh<T>(Volume<T> volume, Func<T, float> densityFunc,
            float isolevel = 0.1f) where T : IVolumeData, new()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var results = MarchingCubes.run(volume, densityFunc, isolevel);
            var mesh = new Mesh();
            

            foreach (var result in results) mesh.AppendTriangle(result.p);
            if (typeof(T) == typeof(Material))
            {
                var materials = volume.Data.Select(x => x.ColorMapping);
                mesh.Colors.AddRange(materials);
                mesh.ColorIndices = Enumerable.Repeat(1, mesh.Indices.Count).ToList();
            }
            sw.Stop();
            
            Console.WriteLine($"MarchingCubes -> Dimensionen:{volume.Dimensions.ToString() + Environment.NewLine } Voxel count: {volume.DataPointers.SelectMany(x => x.Cast<int>()).Count(i => i != 0) + Environment.NewLine}{Environment.NewLine} Vertices: {mesh.Vertices.Count}{Environment.NewLine} Triangles:{mesh.Indices.Count / 3}Elapsed Time: {sw.Elapsed.ToString("g")}");
            return mesh;
        }

        public static Mesh ComputeCubicMesh<T>(Volume<T> volume) where T : IVolumeData, new()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var mesh = new Mesh();
            for (var currentZ = 0; currentZ < volume.Dimensions.Z; currentZ++)
            for (var currentY = 0; currentY < volume.Dimensions.Y; currentY++)
            for (var currentX = 0; currentX < volume.Dimensions.X; currentX++)
            {
                var dataIndex = volume.DataPointers[currentY][currentX, currentZ];
                if (dataIndex == 0) continue;

                var color = volume.Data[dataIndex].ColorMapping;

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
            sw.Stop();
            Console.WriteLine($"Cubic Mesh -> Dimensionen:{volume.Dimensions.ToString() + Environment.NewLine } Voxel count: {volume.DataPointers.SelectMany(x => x.Cast<int>()).Count(i => i != 0) + Environment.NewLine}{Environment.NewLine} Vertices: {mesh.Vertices.Count}{Environment.NewLine} Triangles:{mesh.Indices.Count / 3}Elapsed Time: {sw.Elapsed.ToString("g")}");
            return mesh;
        }

        public static Mesh ComputeCubicMeshGreedy<T>(Volume<T> volume) where T : IVolumeData, new()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var mesh = new Mesh();
            int countX, countY, countZ;
            var _checked = 0;
            var CheckedInVoxels = new bool[volume.Dimensions.Y][,];
            for (var i = 0; i < volume.Dimensions.Y; i++)
                CheckedInVoxels[i] = new bool[volume.Dimensions.X, volume.Dimensions.Z];
            for (var currentZ = 0; currentZ < volume.Dimensions.Z; currentZ++)
            for (var currentY = 0; currentY < volume.Dimensions.Y; currentY++)
            for (var currentX = 0; currentX < volume.Dimensions.X; currentX++)
            {
                var cube = new Vector3[8];
                var currentvoxel = volume.DataPointers[currentY][currentX, currentZ];
                if (currentvoxel == 0 || CheckedInVoxels[currentY][currentX, currentZ])
                    continue;

                _checked++;
                countX = volume.GetSameNeighborsX(currentX, currentY, currentZ, (i, i1, arg3) => true);
                countY = volume.GetSameNeighborsY(currentX, currentY, currentZ, (i, i1, arg3) => true);
                countZ = volume.GetSameNeighborsZ(currentX, currentY, currentZ, (i, i1, arg3) => true);
                if (countX >= countY && countX >= countZ)
                    for (var i = currentX; i <= currentX + countX; i++)
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
                    for (var i = currentZ; i <= currentZ + countZ; i++)
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
                CheckIn(ref volume, ref CheckedInVoxels, new Vector3Int(currentX, currentY, currentZ), end);
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
            sw.Stop();
            Console.WriteLine($"Cubes Greedy -> Dimensionen:{volume.Dimensions.ToString() + Environment.NewLine } Voxel count: {volume.DataPointers.SelectMany(x => x.Cast<int>()).Count(i => i != 0) + Environment.NewLine}{Environment.NewLine} Vertices: {mesh.Vertices.Count}{Environment.NewLine} Triangles:{mesh.Indices.Count / 3}Elapsed Time: {sw.Elapsed.ToString("g")}");
            return mesh;
        }

        private static void CheckIn<T>(ref Volume<T> volume, ref bool[][,] checkedin, Vector3Int start, Vector3Int end)
            where T : IVolumeData, new()
        {
            for (var i = start.X; i < end.X; i++)
            for (var j = start.Y; j < end.Y; j++)
            for (var k = start.Z; k < end.Z; k++)
            {
                var voxel = volume.DataPointers[j][i, k];
                if (volume.Data[voxel].IsSet)
                    checkedin[j][i, k] = true;
            }
        }

        private static void CheckOut<T>(ref bool[][,] checkedin, Vector3Int start, Vector3Int end)
        {
            for (var x = start.X; x < end.X; x++)
            for (var y = start.Y; y < end.Y; y++)
            for (var z = start.Z; z < end.Z; z++)
                checkedin[y][x, z] = false;
        }
        public static Mesh ComputeTRN(VisualizationModel<BaseGridCell> model)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            var heights = model.DataGrid.GetDataGrid(model.HeightMapping);
            var mesh = new GridSurface(heights, model.DataGrid.Width, model.DataGrid.Height);
            sw.Stop();
            Console.WriteLine($@"TRN Dimensionen: X:{heights.GetLength(0)} Z:{heights.GetLength(1)} {Environment.NewLine} Vertices: {mesh.Vertices.Count}{Environment.NewLine} Triangles:{mesh.Indices.Count/3} Elapsed Time: {sw.Elapsed.ToString("g")}");
            return mesh;
        }
    }
}