using System;
using System.Collections.Generic;
using OpenTK;
using SoilSpot.Rendering.DataStructures;

namespace SoilSpot.Rendering.Models.Voxel
{
    public abstract class Volume<T> : RenderObject<T> where T : struct, IVertex
    {
    public bool[][,] CheckedInVoxels;
    public float CubeScale = 1f;
    public Vector3Int Dimensions;
    public List<Material> Materials = new List<Material> {new Material()};
    public byte[][,] VolumeData;

    protected Volume(Vector3Int dimensions)
    {
        this.Dimensions = dimensions;

    }

    protected Volume(int dimX, int dimY, int dimZ)
    {
        Dimensions = new Vector3Int(dimX, dimY, dimZ);
        InitializeVolumeData();
    }

    protected void InitializeVolumeData()
    {
        VolumeData = new byte[Dimensions.Y][,];
        CheckedInVoxels = new bool[Dimensions.Y][,];
        for (var y = 0; y < Dimensions.Y; y++)
        {
            VolumeData[y] = new byte[Dimensions.X, Dimensions.Z];
            CheckedInVoxels[y] = new bool[Dimensions.X, Dimensions.Z];
        }
    }

    public virtual uint GetVoxel(int x, int y, int z)
    {
        return VolumeData[y][x, z];
    }

    public bool IsVoxel(int x, int y, int z)
    {
        return VolumeData[y][x, z] > 0;
    }

    public virtual void SetVoxel(int x, int y, int z, byte materialIndex)
    {
        if (IsValidVoxelPosition(x, y, z))
        {
            VolumeData[y][x, z] = materialIndex;
        }
    }

    public virtual void SetVoxel(int x, int y, int z, Material material)
    {
        if (!Materials.Contains(material)) Materials.Add(material);
        var colorIndex = (byte) Materials.IndexOf(material);

        SetVoxel(x, y, z, colorIndex);
    }

    public virtual void SetVoxel(Vector3 position, Material material)
    {
        SetVoxel((int) position.X, (int) position.Y, (int) position.Z, material);
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
        VolumeData[y][y, z] = 0;
    }

    public abstract void ComputeMesh();

    public void AddMaterial(Material material)
    {
        if (!Materials.Contains(material)) Materials.Add(material);
    }

    public int GetMaterialIndex(Material material)
    {
        return Materials.IndexOf(material);
    }

    public virtual Material GetMaterial(int x, int y, int z)
    {
        if (VolumeData != null)
            return Materials[VolumeData[y][x, z]];
        return Materials[0];
    }

    protected int GetSameNeighborsX(int x, int y, int z)
    {
        var neighborsX = 0;
        while (x < Dimensions.X - 1 && IsSameMaterialRight(x, y, z) &&
               !CheckedInVoxels[y][x + 1, z])
        {
            x++;
            neighborsX++;
        }

        return neighborsX;
    }

    protected int GetSameNeighborsX(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
    {
        var neighborsX = 0;
        while (x < Dimensions.X - 1 && IsSameMaterialRight(x, y, z) &&
               !CheckedInVoxels[y][x + 1, z] && visibilityTestFunction(x, y, z))
        {
            x++;
            neighborsX++;
        }

        return neighborsX;
    }

    protected int GetSameNeighborsY(int x, int y, int z)
    {
        var neighborsY = 0;
        while (y < Dimensions.Y - 1 && IsSameMaterialUp(x, y, z) &&
               !CheckedInVoxels[y + 1][x, z])
        {
            y++;
            neighborsY++;
        }

        return neighborsY;
    }

    protected int GetSameNeighborsY(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
    {
        var neighborsY = 0;
        while (y < Dimensions.Y - 1 && IsSameMaterialUp(x, y, z) &&
               !CheckedInVoxels[y + 1][x, z] && visibilityTestFunction(x, y, z))
        {
            y++;
            neighborsY++;
        }

        return neighborsY;
    }

    protected int GetSameNeighborsZ(int x, int y, int z)
    {
        var neighborsZ = 0;
        while (z < Dimensions.Z - 1 && IsSameMaterialUp(x, y, z) &&
               !CheckedInVoxels[y][x, z + 1])
        {
            z++;
            neighborsZ++;
        }

        return neighborsZ;
    }

    protected int GetSameNeighborsZ(int x, int y, int z, Func<int, int, int, bool> visibilityTestFunction)
    {
        var neighborsZ = 0;
        while (z < Dimensions.Z - 1 && IsSameMaterialUp(x, y, z) &&
               !CheckedInVoxels[y][x, z + 1] && visibilityTestFunction(x, y, z))
        {
            z++;
            neighborsZ++;
        }

        return neighborsZ;
    }

    protected bool IsFrontfaceVisible(int x, int y, int z)
    {
        if (z == 0 || VolumeData[y][x, z - 1] == 0 || Materials[VolumeData[y][x, z - 1]].Color.W != 1f)
            return true;
        return false;
    }

    protected bool IsBackfaceVisible(int x, int y, int z)
    {
        if (z == Dimensions.Z - 1 || VolumeData[y][x, z + 1] == 0 ||
            Materials[VolumeData[y][x, z + 1]].Color.W != 1f) return true;
        return false;
    }

    protected bool IsLeftfaceVisible(int x, int y, int z)
    {
        if (x == 0 || VolumeData[y][x, z] == 0 || Materials[VolumeData[y][x - 1, z]].Color.W != 1f) return true;
        return false;
    }

    protected bool IsRightfaceVisible(int x, int y, int z)
    {
        if (x == Dimensions.X - 1 || VolumeData[y][x + 1, z] == 0 ||
            Materials[VolumeData[y][x + 1, z]].Color.W != 1f) return true;
        return false;
    }

    protected bool IsBottomfaceVisible(int x, int y, int z)
    {
        if (y == 0 || VolumeData[y - 1][x, z] == 0 || Materials[VolumeData[y - 1][x, z]].Color.W != 1f)
            return true;
        return false;
    }

    protected bool IsTopfaceVisible(int x, int y, int z)
    {
        if (y == Dimensions.Y - 1 || VolumeData[y + 1][x, z] == 0 ||
            Materials[VolumeData[y + 1][x, z]].Color.W != 1f) return true;
        return false;
    }

    protected bool IsSameMaterialLeft(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y][x - 1, z];
    }

    protected bool IsSameMaterialUp(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y + 1][x, z];
    }

    protected bool IsSameMaterialDown(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y - 1][x, z];
    }

    protected bool IsSameMaterialFront(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y][x, z + 1];
    }

    protected bool IsSameMaterialBack(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y][x, z - 1];
    }

    protected bool IsSameMaterialRight(int x, int y, int z)
    {
        return VolumeData[y][x, z] == VolumeData[y][x + 1, z];
    }

    public bool IsValidVoxelPosition(int x, int y, int z)
    {
        if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z)
            return true;

        return false;
    }
    }
}