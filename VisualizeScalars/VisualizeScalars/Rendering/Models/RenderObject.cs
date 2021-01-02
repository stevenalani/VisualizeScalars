using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;
using Image = System.Drawing.Image;
using PixelFormat = OpenTK.Graphics.OpenGL4.PixelFormat;

namespace VisualizeScalars.Rendering.Models
{
    public class RenderObject<T> : Model where T : struct, IVertex
    {
        public List<BufferStorage> Buffers = new List<BufferStorage>();

        // Indices
        protected int Ebo = -1;
        public int[] Indices;
        private int TextureID = -1;

        private readonly List<int> Textures = new List<int>();

        // Vertex Array
        protected int Vao = -1;

        // Buffer
        protected int Vbo = -1;
        public T[] Vertices;

        public RenderObject(Mesh mesh, string name, bool isInstanced = false, int instances = 2) : base(name)
        {
            DrawInstanced = isInstanced;
            Instances = instances;
            Mesh = mesh;
            Vertices = Mesh.GetVertices<T>();
            Indices = Mesh.GetIndices();
        }

        public RenderObject(string name, bool isInstanced = false, int instances = 2) : base(name)
        {
            DrawInstanced = isInstanced;
            Instances = instances;
        }

        public bool DrawInstanced { get; set; }
        public int Instances { get; set; }
        public List<Image> Images { get; set; } = new List<Image>();

        private void Gen3DTextures()
        {
            if(Images.Count == 0)return;
            
            var dataList = new List<byte>();
            var newWidth = Images.Sum(x => x.Width) / Images.Count;
            var newHeight = Images.Sum(x => x.Height) / Images.Count;
            Images = Images.Select(image => image.ResizeImage(newWidth, newHeight)).ToList();
            foreach (var image in Images)
            {
                using (var stream = new MemoryStream())
                {
                    image.Save(stream, ImageFormat.Png);
                    stream.Seek(0, SeekOrigin.Begin);
                    Image<Rgba32> tex = SixLabors.ImageSharp.Image.Load<Rgba32>(stream, new PngDecoder());
                    //tex.Mutate(x => x.Flip(FlipMode.Vertical));

                    var pixels = new List<byte>(4 * image.Width * image.Height);

                    for (int y = 0; y < image.Height; y++)
                    {
                        var row = tex.GetPixelRowSpan(y);

                        for (int x = 0; x < image.Width; x++)
                        {
                            pixels.Add(row[x].R);
                            pixels.Add(row[x].G);
                            pixels.Add(row[x].B);
                            pixels.Add(row[x].A);
                        }
                    }
                    dataList.AddRange(pixels);
                }
            }
            var imageBuffer = dataList.ToArray();
            TextureID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture3D, TextureID);
            GL.TexStorage3D(TextureTarget3d.Texture3D, 1, SizedInternalFormat.Rgba32f, newWidth, newHeight,
                Images.Count);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture3D, TextureParameterName.TextureWrapR,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexSubImage3D(TextureTarget.Texture3D, 0, 0, 0, 0, newWidth, newHeight, Images.Count,
                PixelFormat.Rgba, PixelType.UnsignedByte, imageBuffer);
            Textures.Add(TextureID);
        }

        public override void Draw(ShaderProgram shaderProgram, Action<ShaderProgram> setUniforms)
        {
            if (!IsReady) InitBuffers();
            if ( (Indices == null && Mesh == null) || (Mesh != null && Mesh.Indices.Count == 0))
                return;
            setUniforms?.Invoke(shaderProgram);
            shaderProgram.SetUniformMatrix4X4("model", Modelmatrix);
            GL.BindVertexArray(Vao);
            if (TextureID != -1)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture3D, TextureID);
                var img = Images.FirstOrDefault();
                var width = img.Width;
                var height = img.Height;
                shaderProgram.SetUniformVector3("texDimensions",new Vector3(width,height,Images.Count));
            }
            
            // as set in the shadercode
            var binding = 0;
            foreach (var buffer in Buffers)
            {
                shaderProgram.SetUniformFloat($"BufferCnt[{binding}]", buffer.ValueCount);
                buffer.Activate(binding++);
            }

            if (DrawInstanced)
            {
                GL.BindVertexArray(Vao);
                GL.DrawElementsInstanced(PrimitiveType.Triangles, Indices.Length, DrawElementsType.UnsignedInt,
                    IntPtr.Zero, Instances);
            }
            else
            {
                GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
            }

            GL.BindVertexArray(0);
        }

        public void SetMesh(Mesh mesh)
        {
            Mesh = mesh;
            Vertices = Mesh.GetVertices<T>();
            Indices = Mesh.GetIndices();
        }

        public override void InitBuffers()
        {
            if (IsReady)
                return;
            if (Vertices == null && Mesh != null)
            {
                Vertices = Mesh.GetVertices<T>();
                Indices = Mesh.GetIndices();
            }
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();
            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            var data = Vertices.SelectMany(v => v.GetData()).ToArray();
            var dataPointerLength = Vertices[0].GetDataLength();
            GL.BufferData(BufferTarget.ArrayBuffer, data.Length * sizeof(float), data,
                BufferUsageHint.StaticDraw);
            var stride = dataPointerLength.Sum();
            var offset = 0;
            // assigns pointers to vertex attribs like ordered in data from .GetValueArray()
            for (var i = 0; i < dataPointerLength.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, dataPointerLength[i], VertexAttribPointerType.Float, false,
                    sizeof(float) * stride, offset * sizeof(float));
                offset += dataPointerLength[i];
            }

            Gen3DTextures();
            foreach (var buffer in Buffers) buffer.SetUp();
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)),
                Indices.Select(x => (uint) x).ToArray(),
                BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);

            IsReady = true;
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffers(2, new[] {Vbo, Ebo});
            Buffers.ForEach(x => x.Dispose());
        }
    }
}