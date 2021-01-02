using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models.Voxel;

namespace VisualizeScalars.Rendering.Models
{
    public enum FaceOrientation
    {
        Front,
        Back,
        Left,
        Right,
        Bottom,
        Top
    }

    public class CubeWithNormals : PositionColorNormalModel
    {
        public static readonly Vector3[] CubeVertices =
        {
            new Vector3(-0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, -0.5f, -0.5f),
            new Vector3(0.5f, 0.5f, -0.5f),
            new Vector3(-0.5f, 0.5f, -0.5f),

            new Vector3(-0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, -0.5f, 0.5f),
            new Vector3(0.5f, 0.5f, 0.5f),
            new Vector3(-0.5f, 0.5f, 0.5f)
        };

        public CubeWithNormals(Vector3 offset, Vector4 color) :
            base(null, null, "cube")
        {
            var vertices = new List<PositionColorNormalVertex>();
            var normals = new List<Vector3>();
            var indices = new List<int>();
            /* var data = FrontFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = BackFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = LeftFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = RightFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = BottomFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = TopFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             data = BackFace(offset, Color);
             vertices.AddRange(data.Select(d => d.Position));
             normals.AddRange(data.Take(3).Select(x => x.Normal).Distinct());
             normals.AddRange(data.Skip(3).Take(3).Select(x => x.Normal).Distinct());
             */
            vertices.AddRange(BackFace(offset, color));
            vertices.AddRange(FrontFace(offset, color));
            vertices.AddRange(LeftFace(offset, color));
            vertices.AddRange(RightFace(offset, color));
            vertices.AddRange(BottomFace(offset, color));
            vertices.AddRange(TopFace(offset, color));
            Vertices = vertices.ToArray();
            for (var i = 0; i < 6; i++)
                indices.AddRange(new[]
                {
                    0 + 6 * i,
                    1 + 6 * i,
                    2 + 6 * i,
                    2 + 6 * i,
                    3 + 6 * i,
                    0 + 6 * i
                });
            Indices = Enumerable.Range(0, 36).ToArray();
            PrimitiveNormals = normals.ToArray();
        }

        public static Vector3[] CubeScaledVertices(float width, float height, float depth)
        {
            return new[]
            {
                CubeVertices[0],
                CubeVertices[1] + new Vector3(width, 0, 0),
                CubeVertices[2] + new Vector3(width, height, 0),
                CubeVertices[3] + new Vector3(0, height, 0),

                CubeVertices[4] + new Vector3(0, 0, depth),
                CubeVertices[5] + new Vector3(width, 0, depth),
                CubeVertices[6] + new Vector3(width, height, depth),
                CubeVertices[7] + new Vector3(0, height, depth)
            };
        }

        private static Vector3Int correctScales(Vector3Int scales)
        {
            var scales1 = new Vector3Int();
            scales1.X = scales.X == 0 ? 1 : scales.X;
            scales1.Y = scales.Y == 0 ? 1 : scales.Y;
            scales1.Z = scales.Z == 0 ? 1 : scales.Z;
            return scales1;
        }

        public static PositionColorNormalVertex[] FrontFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[0] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[1] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[2] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[2] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[3] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[0] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color}
            };
        }

        public static PositionColorNormalVertex[] BackFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[7] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[6] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[5] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[5] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[4] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[7] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color}
            };
        }

        public static PositionColorNormalVertex[] LeftFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[4] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[0] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[3] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[3] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[7] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[4] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color}
            };
        }

        public static PositionColorNormalVertex[] RightFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[1] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[5] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[6] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[6] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[2] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[1] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color}
            };
        }

        public static PositionColorNormalVertex[] BottomFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[4] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[5] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[1] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[1] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[0] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[4] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color}
            };
        }

        public static PositionColorNormalVertex[] TopFace(Vector3 positionOffset, Vector4 color, float scale = 1f)
        {
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (CubeVertices[3] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[2] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[6] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[6] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[7] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (CubeVertices[3] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color}
            };
        }

        public static PositionColorNormalVertex[] FrontFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[0] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[1] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[2] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[2] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[3] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[0] + positionOffset) * scale, Normal = -Vector3.UnitZ, Color = color}
            };
        }

        public static PositionColorNormalVertex[] BackFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[7] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[6] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[5] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[5] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[4] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[7] + positionOffset) * scale, Normal = Vector3.UnitZ, Color = color}
            };
        }

        public static PositionColorNormalVertex[] LeftFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[4] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[0] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[3] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[3] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[7] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[4] + positionOffset) * scale, Normal = -Vector3.UnitX, Color = color}
            };
        }

        public static PositionColorNormalVertex[] RightFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[1] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[5] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[6] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[6] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[2] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[1] + positionOffset) * scale, Normal = Vector3.UnitX, Color = color}
            };
        }

        public static PositionColorNormalVertex[] BottomFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[4] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[5] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[1] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[1] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[0] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[4] + positionOffset) * scale, Normal = -Vector3.UnitY, Color = color}
            };
        }

        public static PositionColorNormalVertex[] TopFace(Vector3 positionOffset, Vector4 color, float width,
            float height, float depth, float scale = 1f)
        {
            var vertices = CubeScaledVertices(width, height, depth);
            return new[]
            {
                new PositionColorNormalVertex
                    {Position = (vertices[3] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[2] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[6] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[6] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[7] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color},
                new PositionColorNormalVertex
                    {Position = (vertices[3] + positionOffset) * scale, Normal = Vector3.UnitY, Color = color}
            };
        }

        /*public CubeWithNormals() :
            base(null, null, "cube")
        {
            var Color = new Vector4(10, 150, 255, 255) / 255;
            var offset = Vector3.Zero;
            var vertices = new List<PositionColorNormalVertex>();
            var indices = new List<int>();
            vertices.AddRange(FrontFace(offset, Color));
            vertices.AddRange(BackFace(offset, Color));
            vertices.AddRange(LeftFace(offset, Color));
            vertices.AddRange(RightFace(offset, Color));
            vertices.AddRange(BottomFace(offset, Color));
            vertices.AddRange(TopFace(offset, Color));
            Vertices = vertices.ToArray();
            /*for (uint i = 0; i < 6; i++)
            {
                indices.AddRange(new[]{
                    0 + 6 * i,
                    1 + 6 * i,
                    2 + 6 * i,
                    2 + 6 * i,
                    3 + 6 * i,
                    0 + 6 * i});
            }*/
        //for (int i = 0; i < Vertices.Length; i++) indices.Add(i);
        // Indices = indices.ToArray();
        //}

        public override void InitBuffers()
        {
            base.InitBuffers();
        }
    }
}