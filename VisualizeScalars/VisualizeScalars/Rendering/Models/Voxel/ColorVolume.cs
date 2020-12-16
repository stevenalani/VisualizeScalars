using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SoilSpot.Helpers;
using SoilSpot.Rendering.DataStructures;

namespace SoilSpot.Rendering.Models.Voxel
{
    public enum MeshMode{ MarchingCubes, Cubes, GreedyCubes }
    public enum Smoothing { None, Laplacian1 = 1, Laplacian2 = 2, Laplacian5 = 5, Laplacian10 = 10, LaplacianHc1 = 1, LaplacianHc2 = 2, LaplacianHc5 = 5, LaplacianHc10 = 10 }
    public class ColorVolume<T> : Volume<T> where T : struct,IVertex
    {
        protected int voxelCount = 0;
        public MeshMode MeshMode { get; set; } = MeshMode.Cubes;
        public Smoothing Smoothing { get; set; } = Smoothing.None;
        public ColorVolume(int dimensionX, int dimensionY, int dimensionZ, float cubeScale = 1f) : base(dimensionX,
            dimensionY, dimensionZ)
        {
            CubeScale = cubeScale;
        }

        protected void computeMesh()
        {
            if (MeshMode == MeshMode.MarchingCubes)
            {
                ComputeMarchingCubesMesh();
            }
            else if (MeshMode == MeshMode.Cubes)
            {
                ComputeCubicMesh();
            }else if (MeshMode == MeshMode.GreedyCubes)
            {
                ComputeCubicMeshGreedy();
            }
            if (this.Smoothing == Smoothing.Laplacian1 ||
                this.Smoothing == Smoothing.Laplacian2 ||
                this.Smoothing == Smoothing.Laplacian5 ||
                this.Smoothing == Smoothing.Laplacian10 ||
                this.Smoothing == Smoothing.LaplacianHc1 ||
                this.Smoothing == Smoothing.LaplacianHc2 ||
                this.Smoothing == Smoothing.LaplacianHc5 ||
                this.Smoothing == Smoothing.LaplacianHc10)
            {
                if (this.Smoothing == Smoothing.Laplacian1 ||
                    this.Smoothing == Smoothing.Laplacian2 ||
                    this.Smoothing == Smoothing.Laplacian5 ||
                    this.Smoothing == Smoothing.Laplacian10)
                {
                    mesh = MeshSmoother.LaplacianFilter(mesh, (int)this.Smoothing);
                }
                else
                {
                    mesh = MeshSmoother.HCFilter(mesh, (int)this.Smoothing);
                }
            }
        }

        private void ComputeCubicMeshGreedy()
        {
            mesh = new Mesh();
            int countX, countY, countZ;
            var _checked = 0;
            for (var currentZ = 0; currentZ < Dimensions.Z; currentZ++)
            {
                for (var currentY = 0; currentY < Dimensions.Y; currentY++)
                {
                    for (var currentX = 0; currentX < Dimensions.X; currentX++)
                    {
                        Vector3[] cube = new Vector3[8];
                        var currentvoxel = VolumeData[currentY][currentX, currentZ];
                        if (currentvoxel == 0 || CheckedInVoxels[currentY][currentX, currentZ])
                            continue;

                        _checked++;
                        countX = GetSameNeighborsX(currentX, currentY, currentZ);
                        countY = GetSameNeighborsY(currentX, currentY, currentZ);
                        countZ = GetSameNeighborsZ(currentX, currentY, currentZ);
                        if (countX >= countY && countX >= countZ)
                            for (var i = (int)currentX; i <= currentX + countX; i++)
                            {
                                var voxelsAbove = GetSameNeighborsY(i, currentY, currentZ);
                                var voxelsInfront = GetSameNeighborsZ(i, currentY, currentZ);
                                if (voxelsAbove < countY || countY == -1)
                                    countY = voxelsAbove;
                                if (voxelsInfront < countZ || countZ == -1)
                                    countZ = voxelsInfront;
                            }
                        else if (countY >= countX && countY >= countZ)
                            for (var i = currentY; i <= currentY + countY; i++)
                            {
                                var voxelsRight = GetSameNeighborsX(currentX, i, currentZ);
                                var voxelsInfront = GetSameNeighborsZ(currentX, i, currentZ);
                                if (voxelsRight < countX || countX == -1)
                                    countX = voxelsRight;
                                if (voxelsInfront < countZ || countZ == -1)
                                    countZ = voxelsInfront;
                            }
                        else if (countZ >= countX && countZ >= countY)
                            for (var i = (int)currentZ; i <= currentZ + countZ; i++)
                            {

                                var voxelsAbove = GetSameNeighborsY(currentX, currentY, i);
                                var voxelsRight = GetSameNeighborsX(currentX, currentY, i);
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
                        CheckIn(new Vector3Int(currentX, currentY, currentZ), end);
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
        }

        public virtual void ComputeVertices(){}
        private void ComputeMarchingCubesMesh()
        {
            var marchingCubes = new MarchingCubes<T>();
            marchingCubes.run(this);
            var results = marchingCubes.GetResults();
            mesh = new Mesh();
            foreach (var result in results)
            {
                mesh.AppendTriangle<T>(result.p);
            }
        }

        private void ComputeCubicMesh()
        {
            this.mesh = new Mesh();
            for (var currentZ = 0; currentZ < Dimensions.Z; currentZ++)
            {
                for (var currentY = 0; currentY < Dimensions.Y; currentY++)
                {
                    for (var currentX = 0; currentX < Dimensions.X; currentX++)
                    {
                        var colorIndex = VolumeData[currentY][currentX, currentZ];
                        if (colorIndex == 0) continue;

                        Vector4 color = Materials[colorIndex].Color;

                        var offset = new Vector3(currentX, currentY, currentZ);
                        if (IsFrontfaceVisible(currentX, currentY, currentZ))
                        {
                            var width = GetSameNeighborsX(currentX, currentY, currentZ, IsFrontfaceVisible);
                            var height = GetSameNeighborsY(currentX, currentY, currentZ, IsFrontfaceVisible);
                            var depth = GetSameNeighborsZ(currentX, currentY, currentZ, IsFrontfaceVisible);
                            var test = CubeWithNormals.FrontFace(offset, color, width, height, depth, CubeScale);
                            var verts = CubeWithNormals.FrontFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0],verts[1],verts[2]);
                            mesh.AppendTriangle(verts[3],verts[4],verts[5]);
                        }
                        if (IsBackfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.BackFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (IsLeftfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.LeftFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (IsRightfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.RightFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (IsBottomfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.BottomFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                        if (IsTopfaceVisible(currentX, currentY, currentZ))
                        {
                            var verts = CubeWithNormals.TopFace(offset, color, CubeScale);
                            mesh.AppendTriangle(verts[0], verts[1], verts[2]);
                            mesh.AppendTriangle(verts[3], verts[4], verts[5]);
                        }
                    }
                }
            }
        }
        
        public override void InitBuffers()
        {
            ComputeMesh();


            Vertices = mesh.GetVertices<T>();
            Indices = mesh.GetIndices();
            base.InitBuffers();
        }

        

        protected void CheckIn(Vector3Int start, Vector3Int end)
        {
            for (var i =  start.X; i < end.X; i++)
            for (var j =  start.Y; j < end.Y; j++)
            for (var k =  start.Z; k < end.Z; k++)
            {
                var voxel = VolumeData[j][i, k];
                if (voxel != 0)
                    CheckedInVoxels[j][i, k] = true;
            }
        }

        protected void CheckOut(Vector3Int start, Vector3Int end)
        {
            for (var x = (int) start.X; x < end.X; x++)
            for (var y = (int) start.Y; y < end.Y; y++)
            for (var z = (int) start.Z; z < end.Z; z++)
                CheckedInVoxels[y][x, z] = false;
        }


        public int GetVoxelCount()
        {
            var _voxelscount = 0;
            for (var x = 0; x < Dimensions.X; x++)
            for (var y = 0; y < Dimensions.Y; y++)
            for (var z = 0; z < Dimensions.Z; z++)
                if (VolumeData[y][x, z] != 0)
                    _voxelscount++;
            return _voxelscount;
        }

        public int GetCheckedInCount()
        {
            var _voxelscount = 0;
            for (var x = 0; x < Dimensions.X; x++)
            for (var y = 0; y < Dimensions.Y; y++)
            for (var z = 0; z < Dimensions.Z; z++)
                if (CheckedInVoxels[y][x, z])
                    _voxelscount++;
            return _voxelscount;
        }

        public override void ComputeMesh()
        {
            computeMesh();
            //ComputeVertices();
            //ComputeIndices();
        }
    }
}