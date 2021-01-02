using OpenTK;
using VisualizeScalars.Helpers;

namespace VisualizeScalars
{
    public class LightSource
    {
        public Vector3 Color;
        public Vector3 Position;
        public Matrix4 LightSpaceMatrix(float AspectRatio, float ClippingPlaneNear, float ClippingPlaneFar)
        {
            var lightProjection = Matrix4.CreatePerspectiveFieldOfView(45.0f.ToRad(), AspectRatio, ClippingPlaneNear, ClippingPlaneFar);
            var lightView = Matrix4.LookAt(Position, Vector3.Zero, Vector3.UnitY);
            var lightSpaceMatrix = lightProjection * lightView;
            return lightSpaceMatrix;
        }

        public LightSource(Vector3 position, Vector3 color)
        {
            Color = color;
            Position = position;
        }
    }
}