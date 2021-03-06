﻿using System;
using System.Linq;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering.Models
{
    public class PositionColorNormalModel : Model
    {
        // Indices
        protected int Ebo = -1;

        public Mesh mesh;

        // Vertex Array
        protected int Vao = -1;

        // Buffer
        protected int Vbo = -1;
        public PositionColorNormalVertex[] Vertices;

        public PositionColorNormalModel(PositionColorNormalVertex[] vertices, int[] indices,
            string modelname = "untitled")
        {
            Vertices = vertices;
            Indices = indices;
            name = modelname + ID;
        }

        public int Ssbo { get; set; }

        public int[] Indices { get; set; }
        public Vector3[] PrimitiveNormals { get; set; }

        public override void InitBuffers()
        {
            if (IsReady)
                return;
            Vao = GL.GenVertexArray();

            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            Ssbo = GL.GenBuffer();

            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertices.Length * sizeof(float) * 3), Vertices,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)),
                Indices.Select(x => (uint) x).ToArray(),
                BufferUsageHint.StaticDraw);


            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, Ssbo);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, PrimitiveNormals.Length * sizeof(float) * 3,
                PrimitiveNormals, BufferUsageHint.StaticDraw); // allocate 152 bytes of memory
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, Ssbo);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);

            // Colors attribute
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 10, 3 * sizeof(float));

            // Normal
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 7 * sizeof(float));


            GL.BindVertexArray(0);

            IsReady = true;
        }

        public override void Draw(ShaderProgram shader, Action<ShaderProgram> setUniforms)
        {
            if (!IsReady) InitBuffers();
            shader.SetUniformMatrix4X4("model", Modelmatrix);
            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
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

    public class PositionNormalModel : Model
    {
        // Indices
        protected int Ebo = -1;

        public Mesh mesh;

        // Vertex Array
        protected int Vao = -1;

        // Buffer
        protected int Vbo = -1;
        public PositionNormalVertex[] Vertices;

        public PositionNormalModel(PositionNormalVertex[] vertices, int[] indices,
            string modelname = "untitled")
        {
            Vertices = vertices;
            Indices = indices;
            name = modelname + ID;
        }

        public int Ssbo { get; set; }

        public int[] Indices { get; set; }
        public Vector3[] PrimitiveNormals { get; set; }

        public override void InitBuffers()
        {
            if (IsReady)
                return;
            Vao = GL.GenVertexArray();

            Vbo = GL.GenBuffer();
            Ebo = GL.GenBuffer();

            Ssbo = GL.GenBuffer();

            GL.BindVertexArray(Vao);
            GL.BindBuffer(BufferTarget.ArrayBuffer, Vbo);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertices.Length * sizeof(float) * 3), Vertices,
                BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)),
                Indices.Select(x => (uint) x).ToArray(),
                BufferUsageHint.StaticDraw);


            // Vertices positions
            GL.EnableVertexAttribArray(0);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, Ssbo);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, PrimitiveNormals.Length * sizeof(float) * 3,
                PrimitiveNormals, BufferUsageHint.StaticDraw); // allocate 152 bytes of memory
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, 3, Ssbo);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);

            // Colors attribute
            GL.EnableVertexAttribArray(1);
            GL.VertexAttribPointer(1, 4, VertexAttribPointerType.Float, false, sizeof(float) * 10, 3 * sizeof(float));

            // Normal
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(2, 3, VertexAttribPointerType.Float, false, sizeof(float) * 10, 7 * sizeof(float));


            GL.BindVertexArray(0);

            IsReady = true;
        }

        public override void Draw(ShaderProgram shader, Action<ShaderProgram> setUniforms)
        {
            if (!IsReady) InitBuffers();
            shader.SetUniformMatrix4X4("model", Modelmatrix);
            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
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