using OpenTK;

namespace VisualizeScalars.Rendering.DataStructures
{
    public interface IVertex
    {
        public Vector3 Position { get; set; }
        public Vector4 Color { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 TexCoord { get; set; }
        public int[] GetDataLength();
        public float[] GetData();
    }

    public struct PositionColorVertex : IVertex
    {
        private float X;
        private float Y;
        private float Z;
        private float R;
        private float G;
        private float B;
        private float A;

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4 Color
        {
            get => new Vector4(R, G, B, A);
            set
            {
                R = value.X;
                G = value.Y;
                B = value.Z;
                A = value.W;
            }
        }

        public Vector3 Normal
        {
            get => -Vector3.UnitZ;
            set { }
        }

        public Vector2 TexCoord
        {
            get => Vector2.Zero;
            set { }
        }


        public int[] GetDataLength()
        {
            return new[] {3, 4};
        }

        public float[] GetData()
        {
            return new[]
            {
                X, Y, Z,
                R, G, B, A
            };
        }
    }

    public struct PositionNormalVertex : IVertex
    {
        private float X;
        private float Y;
        private float Z;
        private float NX;
        private float NY;
        private float NZ;

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4 Color
        {
            get => Vector4.Zero;
            set { }
        }

        public Vector3 Normal
        {
            get => new Vector3(NX, NY, NZ);
            set
            {
                NX = value.X;
                NY = value.Y;
                NZ = value.Z;
            }
        }

        public Vector2 TexCoord
        {
            get => Vector2.Zero;
            set { }
        }

        public int[] GetDataLength()
        {
            return new[] {3, 3};
        }

        public float[] GetData()
        {
            return new[]
            {
                X, Y, Z,
                NX, NY, NZ
            };
        }
    }

    public struct PositionColorNormalVertex : IVertex
    {
        private float X;
        private float Y;
        private float Z;
        private float R;
        private float G;
        private float B;
        private float A;
        private float NX;
        private float NY;
        private float NZ;

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4 Color
        {
            get => new Vector4(R, G, B, A);
            set
            {
                R = value.X;
                G = value.Y;
                B = value.Z;
                A = value.W;
            }
        }

        public Vector3 Normal
        {
            get => new Vector3(NX, NY, NZ);
            set
            {
                NX = value.X;
                NY = value.Y;
                NZ = value.Z;
            }
        }

        public Vector2 TexCoord
        {
            get => Vector2.Zero;
            set { }
        }

        public int[] GetDataLength()
        {
            return new[] {3, 4, 3};
        }

        public float[] GetData()
        {
            return new[]
            {
                X, Y, Z,
                R, G, B, A,
                NX, NY, NZ
            };
        }
    }

    public struct PositionNormalTexcoordVertex : IVertex
    {
        private float X;
        private float Y;
        private float Z;
        private float NX;
        private float NY;
        private float NZ;
        private float U;
        private float V;

        public Vector3 Position
        {
            get => new Vector3(X, Y, Z);
            set
            {
                X = value.X;
                Y = value.Y;
                Z = value.Z;
            }
        }

        public Vector4 Color
        {
            get => Vector4.Zero;
            set { }
        }

        public Vector3 Normal
        {
            get => new Vector3(NX, NY, NZ);
            set
            {
                NX = value.X;
                NY = value.Y;
                NZ = value.Z;
            }
        }

        public Vector2 TexCoord
        {
            get => new Vector2(U, V);
            set
            {
                U = value.X;
                V = value.Y;
            }
        }

        public int[] GetDataLength()
        {
            return new[] {3, 3, 2};
        }

        public float[] GetData()
        {
            return new[]
            {
                X, Y, Z,
                NX, NY, NZ,
                U, V
            };
        }
    }
}