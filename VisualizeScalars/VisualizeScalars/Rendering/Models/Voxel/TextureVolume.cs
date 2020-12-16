using System;
using System.Collections.Generic;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Tga;
using SixLabors.ImageSharp.PixelFormats;
using SoilSpot.Rendering.ShaderImporter;
using Vector4 = System.Numerics.Vector4;

namespace SoilSpot.Rendering.Models.Voxel
{
    public struct Vector3Int
    {
        public int X;
        public int Y;
        public int Z;

        public static Vector3Int One => new Vector3Int(1,1,1);
        public Vector3Int(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 Vector3 => new Vector3(X, Y, Z);

        public int GetMax()
        {
            return Math.Max(Math.Max(X, Y), Z);
        }

        public override string ToString()
        {
            return $"{X}x{Y}x{Z}";
        }

        public static Vector3 operator /(Vector3Int a, int b)
        {
            return new Vector3(a.X / (float) b, a.Y / (float) b, a.Z / (float) b);
        }

        public static Vector3 operator /(int b, Vector3Int a)
        {
            return new Vector3(b / (float) a.X, b / (float) a.Y, b / (float) a.Z);
        }

        public static Vector3 operator /(Vector3Int a, float b)
        {
            return new Vector3(a.X / b, a.Y / b, a.Z / b);
        }
    }

    public class TextureVolume : Model
    {
        public static readonly uint[] Indices =
        {
            1, 5, 7,
            7, 3, 1,
            0, 2, 6,
            6, 4, 0,
            0, 1, 3,
            3, 2, 0,
            7, 5, 4,
            4, 6, 7,
            2, 3, 7,
            7, 6, 2,
            1, 0, 4,
            4, 5, 1
        };

        protected ShaderProgram BackfaceShader = new ShaderProgram("Shaders\\backface.vs", "Shaders\\backface.fs");

        public bool[,,] CheckedInVoxels;
        public float CubeScale = 1f;

        public Vector3Int Dimensions;
        protected int Ebo;
        protected byte[] ImageBuffer;
        public List<Material> Materials = new List<Material> {new Material()};

        protected ShaderProgram RayMarchingShader =
            new ShaderProgram("Shaders\\raycasting.vs", "Shaders\\raycasting.fs");

        protected int TextureID;

        protected int Vao;
        protected int Vbo;

        /// <summary>
        ///     An Array (y-Dimension) of 2D Arrays (x - Z - plane)
        /// </summary>
        public byte[][,] VolumeData;

        public TextureVolume(int width, int height, int depth, string name = "unnamed")
        {
            Dimensions = new Vector3Int(width, height, depth);
            VolumeData = new byte[height][,];
            for (var i = 0; i < height; i++) VolumeData[i] = new byte[width, depth];

            VolumeScales =
                new Vector3((float) (Dimensions.X * Scales.X), (float) (Dimensions.Y * Scales.Y),
                    (float) (Dimensions.Z * Scales.Z)); /// Dimensions.GetMax();
        }

        private Vector3[] Vertices => new[]
        {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(0.0f, 0.0f, Dimensions.Z),
            new Vector3(0.0f, Dimensions.Y, 0.0f),
            new Vector3(0.0f, Dimensions.Y, Dimensions.Z),
            new Vector3(Dimensions.X, 0.0f, 0.0f),
            new Vector3(Dimensions.X, 0.0f, Dimensions.Z),
            new Vector3(Dimensions.X, Dimensions.Y, 0.0f),
            new Vector3(Dimensions.X, Dimensions.Y, Dimensions.Z)
        };

        public float SamplingRate { get; set; } = 0.5f;
        public Vector3 VolumeScales { get; set; }

        public virtual int GetVoxel(int x, int y, int z)
        {
            return VolumeData[y][x, z];
        }

        public bool IsVoxel(int x, int y, int z)
        {
            return VolumeData[y][x, z] != 0;
        }

        public void SetVoxel(Vector3 position, Material material)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, material);
        }

        public void SetVoxel(Vector3 position, byte materialIndex)
        {
            SetVoxel((int) position.X, (int) position.Y, (int) position.Z, materialIndex);
        }

        public void SetVoxel(int posx, int posy, int posz, Material material)
        {
            if (!Materials.Contains(material)) Materials.Add(material);
            var materialindex = (byte) Materials.IndexOf(material);

            SetVoxel(posx, posy, posz, materialindex);
        }

        public void SetVoxel(int x, int y, int z, byte materialIndex)
        {
            VolumeData[y][x, z] = materialIndex;
        }

        public void ClearVoxel(int x, int y, int z)
        {
            VolumeData[y][x, z] = 0;
        }

        public void AddMaterial(Material material)
        {
            if (!Materials.Contains(material)) Materials.Add(material);
        }

        public byte GetMaterialIndex(Material material)
        {
            return (byte) Materials.IndexOf(material);
        }

        public virtual Material GetMaterial(int x, int y, int z)
        {
            return Materials[VolumeData[y][x, z]];
        }

        protected bool IsSameMaterialLeft(int x, int y, int z)
        {
            return IsValidVoxelPosition(x - 1, y, z) && VolumeData[y][x, z] == VolumeData[y][x - 1, z];
        }

        protected bool IsSameMaterialUp(int x, int y, int z)
        {
            return IsValidVoxelPosition(x, y + 1, z) && VolumeData[y][x, z] == VolumeData[y + 1][x, z];
        }

        protected bool IsSameMaterialDown(int x, int y, int z)
        {
            return IsValidVoxelPosition(x, y - 1, z) && VolumeData[y][x, z] == VolumeData[y - 1][x, z];
        }

        protected bool IsSameMaterialFront(int x, int y, int z)
        {
            return IsValidVoxelPosition(x, y, z + 1) && VolumeData[y][x, z] == VolumeData[y][x, z + 1];
        }

        protected bool IsSameMaterialBack(int x, int y, int z)
        {
            return IsValidVoxelPosition(x, y, z - 1) && VolumeData[y][x, z] == VolumeData[y][x, z - 1];
        }

        protected bool IsSameMaterialRight(int x, int y, int z)
        {
            return IsValidVoxelPosition(x + 1, y, z) && VolumeData[y][x, z] == VolumeData[y][x + 1, z];
        }

        public bool IsValidVoxelPosition(int x, int y, int z)
        {
            if (x >= 0 && x < Dimensions.X && y >= 0 && y < Dimensions.Y && z >= 0 && z < Dimensions.Z)
                return true;

            return false;
        }

        public void ToFile()
        {
            var data = new List<Rgba32>();
            for (var y = 0; y < Dimensions.Y; y++)
            for (var z = 0; z < Dimensions.Z; z++)
            for (var x = 0; x < Dimensions.X; x++)
            {
                var materialColor = GetMaterial(x, y, z).Color / 255;
                var colVec = new Vector4(materialColor.X, materialColor.Y, materialColor.Z, materialColor.W);
                var color = new Rgba32(colVec);

                data.Add(color);
            }

            var width = Dimensions.X * (int) Math.Sqrt(Dimensions.Z);
            var height = Dimensions.Y * (int) Math.Sqrt(Dimensions.Z);
            var pdat = Image.LoadPixelData(data.ToArray(), width, height);
            // = Image.LoadPixelData<Rgba32>(bytes, (int) Math.Sqrt(data.Count), (int) Math.Sqrt(data.Count));
            pdat.Save("D:\\image.tga", new TgaEncoder());
        }

        public override void InitBuffers()
        {
            var data = new List<byte>();
            for (var z = 0; z < Dimensions.Z; z++)
            for (var y = 0; y < Dimensions.Y; y++)
            for (var x = 0; x < Dimensions.X; x++)
            {
                var materialColor = GetMaterial(x, y, z).Color;
                var colVec = new Vector4(materialColor.X, materialColor.Y, materialColor.Z, materialColor.W);
                var color = BitConverter.GetBytes(new Rgba32(colVec.X, colVec.Y, colVec.Z, colVec.W).PackedValue);
                //var color = ((int)materialColor.W << 24) | ((int)materialColor.Z << 16) | ((int)materialColor.Y << 8) | ((int)materialColor.Z);
                data.AddRange(color);
            }

            ImageBuffer = data.ToArray();
            data.Clear();

            TextureID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture3D, TextureID);
            GL.TexStorage3D(TextureTarget3d.Texture3D, 1, SizedInternalFormat.Rgba32f, Dimensions.X, Dimensions.Y,
                Dimensions.Z);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexSubImage3D(TextureTarget.Texture3D, 0, 0, 0, 0, Dimensions.X, Dimensions.Y, Dimensions.Z,
                PixelFormat.Rgba, PixelType.UnsignedByte, ImageBuffer);
            //GL.TexImage3D(TextureTarget.Texture3D,0,PixelInternalFormat.Rgba, Dimensions.X, Dimensions.Y, Dimensions.Z,0, PixelFormat.Rgba,PixelType.UnsignedByte,ImageBuffer);

            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertices.Length * sizeof(float) * 3), Vertices,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)), Indices,
                BufferUsageHint.StaticDraw);


            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
            GL.BindVertexArray(0);
            IsReady = true;
        }

        public override void Draw(ShaderProgram shaderProgram)
        {
            shaderProgram.SetUniformVector3("voldimensions", Dimensions.Vector3);
            shaderProgram.SetUniformVector3("volscale", VolumeScales);
            shaderProgram.SetUniformFloat("samplingrate", SamplingRate);
            shaderProgram.SetUniformMatrix4X4("model", Modelmatrix);
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture3D, TextureID);
            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }
    }
}