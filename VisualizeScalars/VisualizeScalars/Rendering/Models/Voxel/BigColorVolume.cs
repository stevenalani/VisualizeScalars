using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OpenTK;
using SoilSpot.Rendering.DataStructures;

namespace SoilSpot.Rendering.Models.Voxel
{
    public struct Box
    {
        public Box(Vector3 lower, Vector3 upper)
        {
            Lower = lower;
            Upper = upper;
        }

        public Vector3 Lower;
        public Vector3 Upper;

        public Vector3Int LowerInt()
        {
            var lx = (int) Math.Floor(Lower.X);
            var ly = (int) Math.Floor(Lower.Y);
            var lz = (int) Math.Floor(Lower.Z);
            return new Vector3Int(lx > 0 ? lx : 0, ly > 0 ? ly : 0, lz > 0 ? lz : 0);
        }

        public Vector3Int UpperInt()
        {
            var ux = (int) Math.Ceiling(Upper.X);
            var uy = (int) Math.Ceiling(Upper.Y);
            var uz = (int) Math.Ceiling(Upper.Z);
            return new Vector3Int(ux > 0 ? ux : 0, uy > 0 ? uy : 0, uz > 0 ? uz : 0);
        }
    }

    public class BigColorVolume : Volume<PositionColorNormalVertex>
    {
        public static readonly string MapPath = ".\\maps\\";
        public Vector3Int ChunkCount;
        private readonly bool[,,] ChunkHasChanges;
        private readonly BigColorVolumeChunk[,,] Chunks;
        private readonly int ChunkSize;
        private readonly string filename;

        public BigColorVolume(int dimensionsX, int dimensionsY, int dimensionsZ, float cubeScale = 1f,
            int chunksize = 16)
            : base(dimensionsX, dimensionsY, dimensionsZ)
        {
            if (!Directory.Exists(MapPath))
                Directory.CreateDirectory(MapPath);
            filename = $"{DateTime.Now.Millisecond}_{Dimensions.ToString()}.dat";
            File.Create(Path.Combine(MapPath, name));
            ChunkSize = chunksize;
            CubeScale = cubeScale;
            var chunksAlongX = (int) Math.Ceiling(Dimensions.X * 1.0 / chunksize);
            var chunksAlongY = (int) Math.Ceiling(Dimensions.Y * 1.0 / chunksize);
            var chunksAlongZ = (int) Math.Ceiling(Dimensions.Z * 1.0 / chunksize);
            ChunkCount = new Vector3Int(chunksAlongX, chunksAlongY, chunksAlongZ);
            Chunks = new BigColorVolumeChunk[chunksAlongX, chunksAlongY, chunksAlongZ];
            ChunkHasChanges = new bool[chunksAlongX, chunksAlongY, chunksAlongZ];
            InitializeChunks();
        }

        public Box CurrentChunkSpace { get; set; }
        private string FilePath => Path.Combine(MapPath, filename);

        private void InitializeChunks()
        {
            for (uint z = 0; z < ChunkCount.Z; z++)
            for (uint y = 0; y < ChunkCount.Y; y++)
            for (uint x = 0; x < ChunkCount.X; x++)
            {
                var chunk = new BigColorVolumeChunk(ChunkSize, x, y, z, CubeScale);
                Chunks[x, y, z] = chunk;
            }
        }

        public override void SetVoxel(int x, int y, int z, byte materialIndex)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;
            var currentColor = Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetVoxel(voxelPosX, voxelPosY, voxelPosZ);
            if (currentColor != materialIndex)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, materialIndex);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
                IsReady = false;
            }
        }

        public override void SetVoxel(Vector3 position, Material material)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, material);
        }

        public override void SetVoxel(Vector3 position, byte materialIndex)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, materialIndex);
        }

        public override void SetVoxel(int x, int y, int z, Material material)
        {
            var chunkIdxX = x / ChunkSize;
            var chunkIdxY = y / ChunkSize;
            var chunkIdxZ = z / ChunkSize;

            var voxelPosX = x % ChunkSize;
            var voxelPosY = y % ChunkSize;
            var voxelPosZ = z % ChunkSize;


            var currentMaterial = Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].GetMaterial(voxelPosX, voxelPosY, voxelPosZ);
            if (currentMaterial != material || material.Color.W != 0.0f)
            {
                Chunks[chunkIdxX, chunkIdxY, chunkIdxZ].SetVoxel(voxelPosX, voxelPosY, voxelPosZ, material);
                ChunkHasChanges[chunkIdxX, chunkIdxY, chunkIdxZ] = true;
                IsReady = false;
            }
        }

        public override void ClearVoxel(int x, int y, int z)
        {
            SetVoxel(x, y, z, 0);
        }

        public override void ComputeMesh()
        {
            ComputeVertices();
        }

        public override void InitBuffers()
        {
            ComputeMesh();
            base.InitBuffers();
        }

        public void ComputeVertices()
        {
            var _chunks = new List<BigColorVolumeChunk>();
            var vertices = new List<PositionColorNormalVertex>();
            var indices = new List<int>();
            for (var z = 0; z < ChunkCount.Z; z++)
            for (var y = 0; y < ChunkCount.Y; y++)
            for (var x = 0; x < ChunkCount.X; x++)
            {
                var chunk = Chunks[x, y, z];
                _chunks.Add(chunk);
            }

            foreach (var chunk in _chunks)
            {
                var x = chunk.ChunkIdX;
                var y = chunk.ChunkIdY;
                var z = chunk.ChunkIdZ;
                if (ChunkHasChanges[x, y, z])
                {
                    chunk.ComputeVertices();
                    ChunkHasChanges[x, y, z] = false;
                }

                if (chunk.Vertices == null) continue;
                var vertexCount = (uint) vertices.Count;
                vertices.AddRange(chunk.Vertices);
                indices.AddRange(chunk.Indices.Select(index => (int)(index + vertexCount)));
            }

            Vertices = vertices.ToArray();
            Indices = indices.ToArray();

            vertices.Clear();
            indices.Clear();
            _chunks.Clear();
        }


        private void loadChunks()
        {
        }

        public void loadChunk()
        {
            /*FileStream fs = new FileStream(filename, FileMode.Open);
            BinaryReader br = new BinaryReader(fs);
            int streampos = */
        }
    }
}