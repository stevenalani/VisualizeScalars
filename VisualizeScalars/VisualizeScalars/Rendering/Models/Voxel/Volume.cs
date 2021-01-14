using System;
using System.Collections.Generic;
using OpenTK;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public class Volume<T> where T : IVolumeData, new()
    {
        public float CubeScale = 1f;
        protected internal List<T> Data = new List<T> {new T()};
        protected internal int[][,] DataPointers;
        public Vector3Int Dimensions;

        public Volume()
        {
        }

        protected Volume(Vector3Int dimensions)
        {
            Dimensions = dimensions;
        }

        protected Volume(int dimX, int dimY, int dimZ)
        {
            Dimensions = new Vector3Int(dimX, dimY, dimZ);
            InitializeVolumeData();
        }

        protected void InitializeVolumeData()
        {
            Data = new List<T> {new T()};
            DataPointers = new int[Dimensions.Y][,];

            for (var y = 0; y < Dimensions.Y; y++) DataPointers[y] = new int[Dimensions.X, Dimensions.Z];
        }

        public virtual int GetVoxel(int x, int y, int z)
        {
            return DataPointers[y][x, z];
        }

        public bool IsVoxel(int x, int y, int z)
        {
            return DataPointers[y][x, z] > 0;
        }

        public virtual void SetVoxel(int x, int y, int z, int dataIndex)
        {
            if (IsValidVoxelPosition(x, y, z)) DataPointers[y][x, z] = dataIndex;
        }

        public virtual void SetVoxel(int x, int y, int z, T data)
        {
            if (!Data.Contains(data)) Data.Add(data);

            var dataIndex = Data.IndexOf(data);

            SetVoxel(x, y, z, dataIndex);
        }

        public virtual void SetVoxel(Vector3 position, T data)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, data);
        }

        public virtual void SetVoxel(Vector3 position, byte materialIndex)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, materialIndex);
        }

        public void ClearVolume()
        {
            InitializeVolumeData();
        }

        public virtual void ClearVoxel(int x, int y, int z)
        {
            if (!(x <= Dimensions.X && y <= Dimensions.Y && z <= Dimensions.Z) && IsVoxel(x, y, z)) return;
            DataPointers[y][y, z] = 0;
        }

        public void AddMaterial(T material)
        {
            if (!Data.Contains(material)) Data.Add(material);
        }

        public int GetMaterialIndex(T material)
        {
            return Data.IndexOf(material);
        }

        public virtual T GetMaterial(int x, int y, int z)
        {
            if (DataPointers != null)
                return Data[DataPointers[y][x, z]];
            return Data[0];
        }

        public int GetSameNeighborsX(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
        {
            var neighborsX = 0;
            while (x < Dimensions.X - 1 && IsSameMaterialRight(x, y, z) && visibilityTestFunction(x, y, z))
            {
                x++;
                neighborsX++;
            }

            return neighborsX;
        }

        public int GetSameNeighborsY(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
        {
            var neighborsY = 0;
            while (y < Dimensions.Y - 1 && IsSameMaterialUp(x, y, z))
            {
                y++;
                neighborsY++;
            }

            return neighborsY;
        }


        public int GetSameNeighborsZ(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
        {
            var neighborsZ = 0;
            while (z < Dimensions.Z - 1 && IsSameMaterialUp(x, y, z) && visibilityTestFunction(x, y, z))
            {
                z++;
                neighborsZ++;
            }

            return neighborsZ;
        }

        public bool IsFrontfaceVisible(int x, int y, int z)
        {
            if (z == 0 || DataPointers[y][x, z - 1] == 0 || Data[DataPointers[y][x, z - 1]].IsSet)
                return true;
            return false;
        }

        public bool IsBackfaceVisible(int x, int y, int z)
        {
            if (z == Dimensions.Z - 1 || DataPointers[y][x, z + 1] == 0 ||
                Data[DataPointers[y][x, z + 1]].IsSet) return true;
            return false;
        }

        public bool IsLeftfaceVisible(int x, int y, int z)
        {
            if (x == 0 || DataPointers[y][x, z] == 0 || Data[DataPointers[y][x - 1, z]].IsSet) return true;
            return false;
        }

        public bool IsRightfaceVisible(int x, int y, int z)
        {
            if (x == Dimensions.X - 1 || DataPointers[y][x + 1, z] == 0 ||
                Data[DataPointers[y][x + 1, z]].IsSet) return true;
            return false;
        }

        public bool IsBottomfaceVisible(int x, int y, int z)
        {
            if (y == 0 || DataPointers[y - 1][x, z] == 0 || Data[DataPointers[y - 1][x, z]].IsSet)
                return true;
            return false;
        }

        public bool IsTopfaceVisible(int x, int y, int z)
        {
            if (y == Dimensions.Y - 1 || DataPointers[y + 1][x, z] == 0 ||
                Data[DataPointers[y + 1][x, z]].IsSet) return true;
            return false;
        }

        public bool IsSameMaterialLeft(int x, int y, int z)
        {
            return DataPointers[y][x, z] == DataPointers[y][x - 1, z];
        }

        public bool IsSameMaterialUp(int x, int y, int z)
        {
            if (Dimensions.X < x || Dimensions.Y-1 < y+1 || Dimensions.Z < z)
                return false;
            
            return DataPointers[y][x, z] == DataPointers[y + 1][x, z];
        }

        public bool IsSameMaterialDown(int x, int y, int z)
        {
            return DataPointers[y][x, z] == DataPointers[y - 1][x, z];
        }

        public bool IsSameMaterialFront(int x, int y, int z)
        {
            return DataPointers[y][x, z] == DataPointers[y][x, z + 1];
        }

        public bool IsSameMaterialBack(int x, int y, int z)
        {
            return DataPointers[y][x, z] == DataPointers[y][x, z - 1];
        }

        public bool IsSameMaterialRight(int x, int y, int z)
        {
            if (Dimensions.X-1 < x+1 || Dimensions.Y < y || Dimensions.Z < z)
                return false;
            return DataPointers[y][x, z] == DataPointers[y][x + 1, z];
        }

        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z)
                return true;

            return false;
        }
    }
}