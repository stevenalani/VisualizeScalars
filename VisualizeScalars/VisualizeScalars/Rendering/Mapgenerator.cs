using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Processing.Processors.Transforms;
using SoilSpot.Rendering.Models;
using SoilSpot.Rendering.Models.Voxel;
using SoilSpot.Helpers;
using SoilSpot.Rendering.DataStructures;

namespace SoilSpot.Rendering
{
    public static class MaterialGenerator
    {
        private static float Water = 0.1f;
        private static float Sand = 0.2f;
        private static float Dirt = 0.3f;
        private static float Gras = 0.6f;
        private static float Rock = 0.8f;
        private static float Snow = 0.9f;
        private static readonly Material dirt = new Material { Color = new Vector4(170, 170, 170, 255) / 255 };
        private static Material gras = new Material { Color = new Vector4(0, 136, 0, 255) / 255 };

        private static readonly Material rock = new Material { Color = new Vector4(190, 190, 190, 255) / 255 };
        private static readonly Material sand = new Material { Color = new Vector4(134, 100, 71, 255) / 255 };
        private static readonly Material snow = new Material { Color = new Vector4(200, 200, 200, 255) / 255 };
        private static readonly Material water = new Material { Color = new Vector4(177, 159, 144, 255) / 255 };

        private static readonly Material waterl = new Material { Color = new Vector4(0, 151, 255, 127) / 255 };



        public static Material Material(float y, int deltaheight)
        {
            var heightpitch = 1f; //;rand.NextDouble() * 2 - 1.0;
            var heightcolorscale = 1.0;
            while (heightcolorscale * deltaheight * Water < 1) heightcolorscale *= 1.1f;
            Material color;
            if (y - heightpitch <= deltaheight * Water * heightcolorscale)
                color = water;
            else if (y - heightpitch > deltaheight * Water * heightcolorscale &&
                     y - heightpitch <= deltaheight * Sand * heightcolorscale)
                color = water;
            else if (y - heightpitch > deltaheight * Sand * heightcolorscale &&
                     y - heightpitch <= deltaheight * Dirt * heightcolorscale)
                color = sand;
            else if (y - heightpitch > deltaheight * Dirt * heightcolorscale &&
                     y - heightpitch <= deltaheight * Gras * heightcolorscale)
                color = dirt;
            else if (y - heightpitch > deltaheight * Gras * heightcolorscale &&
                     y - heightpitch <= deltaheight * Rock * heightcolorscale)
                color = gras;
            else if (y - heightpitch > deltaheight * Rock * heightcolorscale &&
                     y - heightpitch <= deltaheight * Snow * heightcolorscale)
                color = rock;
            else
                color = snow;
            return color;
        }
    }
    public class Mapgenerator
    {
        private short[,] heights;
        private int rowCount;
        private int colCount;

        public Mapgenerator(short[,] heights)
        {
            this.heights = heights;
        }
        private int Y(int x, double slope, int y0)
        {
            return (int)Math.Round(x * slope + y0);
        }
        public TextureVolume GenerateTerrain(short[,] heights)
        {
            heights = heights.Scale(1);
            rowCount = heights.GetLength(0); 
            colCount = heights.GetLength(1);
            int maxVal = heights[0, 0];
            int minVal = heights[0, 0];
            for (var y = 0; y < rowCount; y++)
            for (var x = 0; x < colCount; x++)
            {
                var val = heights[y, x];
                if (maxVal < val) maxVal = val;
                if (minVal > val) minVal = val;
            }

            var deltaheight = maxVal - minVal;
            var vol = new TextureVolume(rowCount, deltaheight + 2, colCount);

            for (var z = 0; z < colCount; z++)
            for (var x = 0; x < rowCount; x++)
            {
                var minNeighbour = getNeighbours(ref heights, x, z).Min();
                for (var i = minNeighbour - minVal; i <= heights[x, z] - minVal; i++)
                    vol.SetVoxel(x, i, z, MaterialGenerator.Material(i, deltaheight));
            }

            return vol;
        }
        
        public ColorVolume<PositionColorNormalVertex> GenerateMapFromHeightData(int scale = 1)
        {
            short[,] height;
            if (scale > 1)
            {
                height = heights.Scale(scale);
            }
            else
            {
                height = heights;
            }
            
            colCount = height.GetLength(1);
            rowCount = height.GetLength(0);
            int maxVal = height[0, 0];
            int minVal = height[0, 0];
            var mat = new Material(){Color = new Vector4(0.5f,0.5f,0.5f,1f)};
            for (var z = 1; z < colCount; z++)
            for (var x = 1; x < rowCount; x++)
            {
                if (maxVal < height[x, z]) maxVal = height[x, z];
                if (minVal > height[x, z]) minVal = height[x, z];
            }
            var deltaheight = maxVal - minVal;
            var volume = new ColorVolume<PositionColorNormalVertex>(rowCount+2, deltaheight+4, colCount+2);
            for (var z = 0; z < colCount; z++)
            for (var x = 0; x < rowCount; x++)
            {
                var minNeighbour = getNeighbours(ref height,x,z).Min();
                for (var i = minNeighbour - minVal-1; i <= height[x, z] - minVal; i++)
                {
                    volume.SetVoxel(x+1, i+1, z+1, mat);
                }
            }

            volume.Position = new OpenTK.Vector3(volume.Dimensions.X/-2f,-minVal,volume.Dimensions.Z/-2f);
            volume.Scales = new OpenTK.Vector3(30f/scale,1f,30f/scale) /10;
            volume.Rotations.X = 1f;
            return volume;
        }

        private int[] getNeighbours(ref short[,] heights,int x,int z)
        {
            int[] neighbours = null;
            if (x == 0 && z == 0)
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x + 1, z + 1],
                        heights[x + 1, z]
                };
            else if (x == 0 && z < colCount - 1)
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x + 1, z + 1],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x == 0 && z == colCount - 1)
                neighbours = new int[]
                {
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x < rowCount - 1 && z == 0)
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x + 1, z + 1],
                        heights[x - 1, z],
                        heights[x + 1, z]
                };
            else if (x < rowCount - 1 && z == colCount - 1)
                neighbours = new int[]
                {
                        heights[x - 1, z],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1],
                        heights[x + 1, z - 1]
                };
            else if (x == rowCount - 1 && z == 0)
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x - 1, z]
                };
            else if (x == rowCount - 1 && z < colCount - 1)
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x - 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1]
                };
            else if (x == rowCount - 1 && z == colCount - 1)
                neighbours = new int[]
                {
                        heights[x - 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1]
                };
            else
                neighbours = new int[]
                {
                        heights[x, z + 1],
                        heights[x - 1, z + 1],
                        heights[x + 1, z + 1],
                        heights[x - 1, z],
                        heights[x + 1, z],
                        heights[x, z - 1],
                        heights[x - 1, z - 1],
                        heights[x + 1, z - 1]
                };

            return neighbours;
        }
    }
}