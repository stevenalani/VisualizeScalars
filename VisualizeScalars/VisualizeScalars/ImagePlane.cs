using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models;
using VisualizeScalars.Rendering.ShaderImporter;
using Image = SixLabors.ImageSharp.Image;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace VisualizeScalars
{
    public class ImagePlane : RenderObject<PositionNormalTexcoordVertex>
    {
        public readonly int Height;
        private readonly byte[] ImageData;
        private int TexID;
        public readonly int Width;

        public ImagePlane(Bitmap bitmap) : base("map")
        {
            this.Width = bitmap.Width;
            this.Height = bitmap.Height;
            PivotPoint.X = Width / 2.0f;
            PivotPoint.Z = Height / 2.0f;
            Vertices = new[]
            {
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f, 0.0f, 0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, 0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 1.0f)
                },

                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 1.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 1.0f)
                }
            };
            Indices = new[] {1,2,3,4,5,6};
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            stream.Seek(0, SeekOrigin.Begin);
            Image<Rgba32> image = Image.Load<Rgba32>(stream,new PngDecoder());
            var pixels = new List<byte>(4 * image.Width * image.Height);

            for (int y = 0; y < image.Height; y++)
            {
                var row = image.GetPixelRowSpan(y);

                for (int x = 0; x < image.Width; x++)
                {
                    pixels.Add(row[x].R);
                    pixels.Add(row[x].G);
                    pixels.Add(row[x].B);
                    pixels.Add(row[x].A);
                }
            }

            ImageData = pixels.ToArray();
        }

        public override void InitBuffers()
        {
            
            base.InitBuffers();
            TexID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TexID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba32f, Width, Height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, ImageData);
        }

        public override void Draw(ShaderProgram shaderProgram, Action<ShaderProgram> setUniforms)
        {
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TexID);
            base.Draw(shaderProgram, setUniforms);
            shaderProgram.SetUniformInt("tex", 0);
        }
    }
}