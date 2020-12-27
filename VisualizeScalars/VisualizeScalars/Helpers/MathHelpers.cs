using OpenTK;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Helpers
{
    public static class MathHelpers
    {
        public static Matrix4 GetRotation(float yaw, float pitch, float roll)
        {
            var qPitch = Quaternion.FromAxisAngle(Vector3.UnitX, pitch);
            var qYaw = Quaternion.FromAxisAngle(Vector3.UnitY, yaw);
            var qRoll = Quaternion.FromAxisAngle(Vector3.UnitZ, roll);
            var orientation = qPitch * qYaw * qRoll;
            orientation = Quaternion.Normalize(orientation);
            var rotate = Matrix4.CreateFromQuaternion(orientation);
            return rotate;
        }

        public static Vector3 GetSurfaceNormalNewell(PositionColorVertex[] orderedVertices)
        {
            var normal = Vector3.Zero;
            for (var i = 0; i < orderedVertices.Length - 1; i++)
            {
                var CurrentVertex = orderedVertices[i].Position;
                var NextVertex = orderedVertices[i + 1].Position;

                normal.X += (CurrentVertex.Y - NextVertex.Y) * (CurrentVertex.Z + NextVertex.Z);
                normal.Y += (CurrentVertex.Z - NextVertex.Z) * (CurrentVertex.X + NextVertex.X);
                normal.X += (CurrentVertex.X - NextVertex.X) * (CurrentVertex.Y + NextVertex.Y);
            }

            return normal.Normalized();
        }

        public static Vector3 GetSurfaceNormal(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            var u = p2 - p1;
            var v = p3 - p1;
            var normal = Vector3.Cross(v, u);

            return normal.Normalized();
        }

        public static double NormalizeValue(double val, double min, double max)
        {
            if (val == 0.0 || double.IsNaN(val) || max == 0.0)
                return 0.0;
            return (val - min) / (max - min);
        }

        public static double NormalizeValue(double val, double max)
        {
            return NormalizeValue(val, 0, max);
        }

        public static Vector3 GetIntersection(Vector3 rayDirection, Vector3 rayPoint, Vector3 planePoint,
            Vector3 planeNormal)
        {
            var difference = rayDirection - planePoint;
            var dot1 = Vector3.Dot(difference, planeNormal);
            var dot2 = Vector3.Dot(rayDirection, planeNormal);
            var length = dot1 / dot2;
            var intersection = rayPoint - rayDirection * length;
            return intersection;
        }

        public static Vector3 GetIntersection2(Vector3 rayDirection, Vector3 rayPoint, Vector3 planePoint,
            Vector3 planeNormal)
        {
            rayDirection.Normalize();
            double t = (Vector3.Dot(planeNormal, planePoint) - Vector3.Dot(planeNormal, rayPoint)) /
                       Vector3.Dot(planeNormal, rayDirection);
            return rayPoint + Vector3.Multiply(rayDirection, (float) t);

            var difference = rayDirection - planePoint;
            var dot1 = Vector3.Dot(difference, planeNormal);
            var dot2 = Vector3.Dot(rayDirection, planeNormal);
            var length = dot1 / dot2;
            var intersection = rayPoint - rayDirection * length;
            return intersection;
        }
    }
}