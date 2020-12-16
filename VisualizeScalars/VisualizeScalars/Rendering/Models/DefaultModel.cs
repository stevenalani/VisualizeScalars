using System;
using OpenTK.Graphics.OpenGL4;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering.Models
{
    public class DefaultModel : Model
    {
        private readonly int[] _indices;
        private readonly int[] _vertices;
        protected int Ebo = -1;

        protected int Vao = -1;
        protected int Vbo = -1;

        public PositionColorVertex[] Vertices;

        public DefaultModel(int[] vertices, int[] indices, int[] normals)
        {
            _vertices = vertices;
            _indices = indices;
            IndicesCnt = indices.Length;
        }

        public float[] VertexData { get; set; }
        public float[] IndexData { get; set; }

        public int IndicesCnt { get; set; }

        public override void Draw(ShaderProgram shader)
        {
            GL.UseProgram(shader.ID);
            GL.DrawElements(BeginMode.Triangles,
                IndicesCnt,
                DrawElementsType.UnsignedInt,
                0
            );
        }


        public override void InitBuffers()
        {
            Vao = GL.GenVertexArray();
            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);

            GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float) * 7, _vertices,
                BufferUsageHint.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(int), _indices,
                BufferUsageHint.StaticDraw);

            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, IntPtr.Zero);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 4, IntPtr.Zero);


            GL.BindVertexArray(0);
            IsReady = true;
        }


        public bool Ready()
        {
            return IsReady;
        }

        public override void Dispose()
        {
            if (IsReady)
            {
                GL.DeleteVertexArray(Vao);
                GL.DeleteBuffer(Vbo);
                IsReady = false;
            }
        }
    }
}