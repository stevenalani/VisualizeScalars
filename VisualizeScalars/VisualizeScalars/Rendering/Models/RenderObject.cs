using System;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering.Models
{
    public class RenderObject<T> : Model where T : struct, IVertex
    {
        public T[] Vertices;
        public int[] Indices;
        public Mesh mesh;
        // Indices
        protected int Ebo = -1;
        // Vertex Array
        protected int Vao = -1;
        // Buffer
        protected int Vbo = -1;
        public override void Draw(ShaderProgram shaderProgram)
        {
            if (!IsReady) InitBuffers();
            shaderProgram.SetUniformMatrix4X4("model", Modelmatrix);
            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void InitBuffers()
        {
            if (IsReady)
                return;
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
            int offset = 0;
            // assigns pointers to vertex attribs like ordered in data from .GetValueArray()
            for (int i = 0; i < dataPointerLength.Length; i++)
            {
                GL.EnableVertexAttribArray(i);
                GL.VertexAttribPointer(i, dataPointerLength[i], VertexAttribPointerType.Float, false, sizeof(float) * stride , offset * sizeof(float));
                offset += dataPointerLength[i];
            }
            /*
            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, 0);

           // Normal
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, sizeof(float) * 6, 3 * sizeof(float));
            */
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)), Indices.Select(x => (uint)x).ToArray(),
                BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);

            IsReady = true;
        }
    }
}