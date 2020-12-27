using System;
using System.Linq;
using OpenTK.Graphics.OpenGL4;

namespace VisualizeScalars.Rendering.Models
{
    public class BufferStorage : IDisposable
    {
        private static int bufferIndices = 2;
        private readonly byte[] bufferData;
        private readonly Type type;

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

        public int BufferID { get; private set; }
        public int BufferIndex { get; }
        public int ValueCount { get; set; }

        public bool IsReady { get; set; }

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

        public void Dispose()
        {
            GL.DeleteBuffer(BufferID);
        }

        public void Activate(int binding)
        {
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, BufferID);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, new IntPtr(bufferData.Length * getSize), bufferData,
                BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, binding, BufferID);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
        }

        public void SetUp()
        {
            if (IsReady) return;
            BufferID = GL.GenBuffer();
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, BufferID);
            GL.BufferData(BufferTarget.ShaderStorageBuffer, bufferData.Length * getSize, bufferData,
                BufferUsageHint.StaticDraw);
            GL.BindBufferBase(BufferRangeTarget.ShaderStorageBuffer, BufferIndex, BufferID);
            GL.BindBuffer(BufferTarget.ShaderStorageBuffer, 0);
            IsReady = true;
        }
    }
}