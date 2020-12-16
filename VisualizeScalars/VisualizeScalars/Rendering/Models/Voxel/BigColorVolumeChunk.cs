using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SoilSpot.Rendering.DataStructures;

namespace SoilSpot.Rendering.Models.Voxel
{
    public class BigColorVolumeChunk : ColorVolume<PositionColorNormalVertex>
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
            Position.X = dimension * idX * CubeScale + Position.X;
            Position.Y = dimension * idY * CubeScale + Position.Y;
            Position.Z = dimension * idZ * CubeScale + Position.Z;
            VolumeData = null;
        }

        public override void ComputeVertices()
        {
            throw new NotImplementedException();
        }

        public override void SetVoxel(int x, int y, int z, Material material)
        {
            CreateVolDataIfNull();
            base.SetVoxel(x, y, z, material);
        }

        private void CreateVolDataIfNull()
        {
            if (VolumeData == null) InitializeVolumeData();
        }

        public override void SetVoxel(int x, int y, int z, byte materialIndex)
        {
            CreateVolDataIfNull();
            base.SetVoxel(x, y, z, materialIndex);
        }

        public override void SetVoxel(Vector3 position, Material material)
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
            if (voxelCount == 0) VolumeData = null;
        }
    }
}