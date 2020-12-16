using System.Linq;
using OpenTK;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models
{
    public class BoundingBox : Cube
    {
        public Vector4 Color;

        public Vector3 leftlowfar;
        public Vector3 leftlownear;
        public Vector3 leftupfar;
        public Vector3 leftupnear;
        public Vector3 rightlowfar;
        public Vector3 rightlownear;
        public Vector3 rightupfar;
        public Vector3 rightupnear;
        public Surface[] Surfaces = new Surface[6];

        public BoundingBox(Model model, Vector4 color = default)
        {
            purgesiblings = true;
            Color = color == default ? new Vector4(25.5f, 178f, 255f, 25f) : color;
            if (model is PositionColorNormalModel)
            {
                update((PositionColorNormalModel) model);
            }
        }

        public string series { get; set; } = "default";
        public bool purgesiblings { get; set; }

        private bool IsInX(Vector3 intersection)
        {
            return intersection.X >= leftlownear.X && intersection.X <= rightlownear.X;
        }

        private bool IsInY(Vector3 intersection)
        {
            return intersection.Y >= leftlownear.Y && intersection.Y <= rightupnear.Y;
        }

        private bool IsInZ(Vector3 intersection)
        {
            return leftlownear.Z >= intersection.Z && rightlowfar.Z <= intersection.Z;
        }

        private void update(PositionColorNormalModel inmodel)
        {
            Position = inmodel.Position;
            Rotations = inmodel.Rotations;
            Scales = inmodel.Scales;
            if (inmodel.IsReady)
            {
                var vertices = inmodel.Vertices.Select(x => Vector3.TransformPosition(x.Position, inmodel.Modelmatrix));
                var minY = vertices.Min(x => x.Y);
                var maxY = vertices.Max(x => x.Y);
                var minX = vertices.Min(x => x.X);
                var maxX = vertices.Max(x => x.X);
                var minZ = vertices.Min(x => x.Z);
                var maxZ = vertices.Max(x => x.Z);

                var size = new Vector3(maxX - minX, maxY - minY, maxZ - minZ);

                leftlownear = new Vector3(minX, minY, maxZ);
                rightlownear = new Vector3(maxX, minY, maxZ);
                rightupnear = new Vector3(maxX, maxY, maxZ);
                leftupnear = new Vector3(minX, maxY, maxZ);

                leftlowfar = new Vector3(minX, minY, minZ);
                rightlowfar = new Vector3(maxX, minY, minZ);
                rightupfar = new Vector3(maxX, maxY, minZ);
                leftupfar = new Vector3(minX, maxY, minZ);

                Surfaces = new[]
                {
                    new Surface(leftlownear, rightlownear, rightupnear),
                    new Surface(rightlownear, rightlowfar, rightupfar),
                    new Surface(rightlowfar, leftlowfar, leftupfar),
                    new Surface(leftlowfar, leftlownear, leftupnear),
                    new Surface(leftupnear, rightupnear, rightupfar),
                    new Surface(leftlownear, rightlownear, rightlowfar)
                };

                Vertices = ToArray()?.Select(x => new PositionColorVertex
                {
                    Position = Vector3.TransformPosition(x, inmodel.Modelmatrix.Inverted()),
                    Color = Color / 255
                }).ToArray();
                IsReady = false;
            }
        }


        public Vector3[] ToArray()
        {
            return new[]
            {
                leftlownear,
                rightlownear,
                rightupnear,
                leftupnear,
                leftlowfar,
                rightlowfar,
                rightupfar,
                leftupfar
            };
        }

        public Vector3[] ContainsVector(IntersectionResult[] intersectionResults)
        {
            //List<Vector3> hits = new List<Vector3>();
            var hits = intersectionResults.Where(x =>
                IsInX(x.Intersection) && IsInY(x.Intersection) && IsInZ(x.Intersection)).Select(x => x.Intersection);
            /*foreach (var intersectionResult in intersectionResults)
            {
                var inX = IsInX(intersectionResult.Intersection);
                var inY = IsInY(intersectionResult.Intersection);
                var inZ = IsInZ(intersectionResult.Intersection);
                if (inX && inY && inZ)
                {
                    hits.Add(intersectionResult.Intersection);
                }
            }*/
            return hits.ToArray();
        }
    }

    public class Surface
    {
        public Vector3 Normal;
        public Vector3 Point;

        public Surface(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            Normal = MathHelpers.GetSurfaceNormal(p1, p2, p3);
            Point = p1;
        }
    }
}