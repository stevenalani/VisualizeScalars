using OpenTK;

namespace VisualizeScalars.Rendering.DataStructures
{
    public interface IVertex
    {
        public int[] GetDataLength();
    }

    public struct PositionVertex : IVertex
    {
        public Vector3 Position;
        public int[] GetDataLength()
        {
            return new[] {3};
        }

        public float[] GetValueArray()
        {
            return new[] {Position.X, Position.Y, Position.Z};
        }
    }
    public struct PositionColorVertex : IVertex
    {
        public Vector3 Position;
        public Vector4 Color;
        public int[] GetDataLength()
        {
            return new[] { 3, 4 };
        }


    }
    public struct PositionNormalVertex : IVertex
    {
        public Vector3 Position;
        public Vector3 Normal;

        public int[] GetDataLength()
        {
            return new[] {3, 3};
        }
    }
    public struct PositionColorNormalVertex : IVertex
    {
        public Vector3 Position;
        public Vector4 Color;
        public Vector3 Normal;
        public int[] GetDataLength()
        {
            return new[] { 3, 4, 3 };
        }
    }


}