using System;
using OpenTK;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public class BigColorVolumeChunk<T> : ColorVolume<T> where T : IVolumeData, new()
    {
        internal uint ChunkIdX;
        internal uint ChunkIdY;
        internal uint ChunkIdZ;

        public BigColorVolumeChunk(int dimension, uint idX, uint idY, uint idZ, float cubeScale = 1f) : base(dimension,
            dimension, dimension, cubeScale)
        {
            ChunkIdX = idX;
            ChunkIdY = idY;
            ChunkIdZ = idZ;
           /* Position.X = dimension * idX * CubeScale + Position.X;
            Position.Y = dimension * idY * CubeScale + Position.Y;
            Position.Z = dimension * idZ * CubeScale + Position.Z;*/
            DataPointers = null;
        }



        public override void SetVoxel(int x, int y, int z, T material)
        {
            CreateVolDataIfNull();
            base.SetVoxel(x, y, z, material);
        }

        private void CreateVolDataIfNull()
        {
            if (DataPointers == null) InitializeVolumeData();
        }

        public override void SetVoxel(int x, int y, int z, int materialIndex)
        {
            CreateVolDataIfNull();
            base.SetVoxel(x, y, z, materialIndex);
        }

        public override void SetVoxel(Vector3 position, T material)
        {
            CreateVolDataIfNull();
            base.SetVoxel(position, material);
        }

        public override void SetVoxel(Vector3 position, byte materialIndex)
        {
            CreateVolDataIfNull();
            base.SetVoxel(position, materialIndex);
        }

        public override void ClearVoxel(int x, int y, int z)
        {
            base.ClearVoxel(x, y, z);
            if (voxelCount == 0) DataPointers = null;
        }
    }
}