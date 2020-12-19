using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK.Graphics.OpenGL4;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.ShaderImporter;
using Buffer = OpenTK.Graphics.OpenGL4.Buffer;

namespace VisualizeScalars.Rendering.Models
{
    public class RenderObject<T> : Model where T : struct, IVertex
    {
        public T[] Vertices;
        public int[] Indices;

        public List<BufferStorage> Buffers = new List<BufferStorage>();
        // Indices
        protected int Ebo = -1;
        // Vertex Array
        protected int Vao = -1;
        // Buffer
        protected int Vbo = -1;

        public RenderObject(Mesh mesh,string name):base(name)
        {
            this.Mesh = mesh;
        }
        public override void Draw(ShaderProgram shaderProgram,Action<ShaderProgram> setUniforms)
        {
            if (!IsReady) InitBuffers();
            setUniforms?.Invoke(shaderProgram);
            shaderProgram.SetUniformMatrix4X4("model", Modelmatrix);
            GL.BindVertexArray(Vao);
            GL.DrawElements(BeginMode.Triangles, Indices.Length, DrawElementsType.UnsignedInt, 0);
        }

        public override void InitBuffers()
        {
            if (IsReady || Mesh.Indices.Count == 0)
                return;
            Vertices = Mesh.GetVertices<T>();
            Indices = Mesh.GetIndices();
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

            foreach (var buffer in Buffers)
            {
                buffer.SetUp();
            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, Ebo);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(Indices.Length * sizeof(uint)), Indices.Select(x => (uint)x).ToArray(),
                BufferUsageHint.StaticDraw);
            GL.BindVertexArray(0);

            IsReady = true;
        }

        public override void Dispose()
        {
            GL.DeleteVertexArray(Vao);
            GL.DeleteBuffers(2,new []{Vbo, Ebo});
            Buffers.ForEach(x => x.Dispose());
        }
    }

    public class BufferStorage : IDisposable
    {
        private static int bufferIndices = 2;
        public int BufferID { get; private set; }
        private int BufferIndex { get; }
        public int ValueCount { get; set; }
        byte[] bufferData;
        private Type type;
        
        public bool IsReady { get; set; }
         
        public BufferStorage(float[] data)
        {
            BufferIndex = bufferIndices++;
            ValueCount = data.Length;
            bufferData = data.SelectMany(x => BitConverter.GetBytes(x)).ToArray();
            type = typeof(float);
        }
        public BufferStorage(int[] data)
        {
            BufferIndex = bufferIndices++;
            ValueCount = data.Length;
            type = typeof(float);
            bufferData = data.SelectMany(x => BitConverter.GetBytes(x)).ToArray();

        }
        public BufferStorage(byte[] data)
        {
            BufferIndex = bufferIndices++;
            bufferData = data;
            ValueCount = data.Length;
            type = typeof(byte);
        }

        public void Activate()
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, BufferID);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, new IntPtr(bufferData.Length * getSize), bufferData, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, BufferIndex, BufferID);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public int getSize
        {
            get
            {
                if (type == typeof(float))
                    return sizeof(float); 
                if (type == typeof(int))
                    return sizeof(int);
                return 1;
            }
        }

        public void SetUp()
        {
            if(IsReady) return;
            BufferID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, BufferID);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, bufferData.Length * getSize, bufferData, BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, BufferIndex, BufferID);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
            IsReady = true;
        }

        public void Dispose()
        {
            GL.DeleteBuffer(BufferID);
        }
    }
    
}