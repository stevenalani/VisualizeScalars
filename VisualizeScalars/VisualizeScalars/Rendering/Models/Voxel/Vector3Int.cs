using System;
using OpenTK;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public struct Vector3Int
    {
        public int X;
        public int Y;
        public int Z;

        public static Vector3Int One => new Vector3Int(1, 1, 1);

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
            return $"X: {X} Y: {Y} Z: {Z}";
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
}