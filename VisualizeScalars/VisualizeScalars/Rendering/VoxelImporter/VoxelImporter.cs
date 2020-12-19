using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using OpenTK;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models.Voxel;

namespace VisualizeScalars.Rendering.VoxelImporter
{
    public static class VoxelImporter
    {
        public static ColorVolume<Material> LoadVoxelModelFromVox(string path)
        {
            var colorsList = new List<Vector4>();

            var data = File.ReadAllBytes(path);
            var id = Encoding.UTF8.GetString(data.Take(4).ToArray());
            var version = BitConverter.ToInt32(data.Skip(4).Take(4).ToArray(), 0);
            var chunk = new MainChunk(data.Skip(8).ToArray());
            var sizeInformation = (SizeChunk) chunk.ChildChunks.Select(x => x).First(x => x is SizeChunk);
            var colorInformation = (RgbaChunk) chunk.ChildChunks.Select(x => x).First(x => x is RgbaChunk);
            var voxelInformation = (XyziChunk) chunk.ChildChunks.Select(x => x).First(x => x is XyziChunk);
            var dimensions = new Vector3(sizeInformation.X, sizeInformation.Y, sizeInformation.Z);
            var vol = new ColorVolume<Material>((int) dimensions.X, (int) dimensions.X, (int) dimensions.X);

            foreach (var color in colorInformation.RGBA)
                colorsList.Add(new Vector4(color.Item1, color.Item2, color.Item3, color.Item4));

            foreach (var voxel in voxelInformation.Voxels)
                vol.SetVoxel(new Vector3(dimensions.X - voxel.Item1 - 1, voxel.Item3, voxel.Item2),
                    new Material {Color = colorsList[voxel.Item4]});
            return vol;
        }
    }
}