using System;
using OpenTK;

namespace VisualizeScalars.Rendering
{
    public partial class Camera
    {
        public double Pitch = 0f;
        public double Sensitivity = SENSITIVITY;

        public double Yaw = 0f;

        internal Vector3 Target => Position + ViewDirection;

        private Matrix4 GetRotationMatrix()
        {
            var qYaw = Quaternion.FromAxisAngle(Vector3.UnitY, (float) (Yaw * (Math.PI / 180)));
            var qPitch = Quaternion.FromAxisAngle(Vector3.UnitX, (float) (Pitch * (Math.PI / 180)));
            var orientation = qPitch * qYaw;
            orientation = Quaternion.Normalize(orientation);
            var rotate = Matrix4.CreateFromQuaternion(orientation);
            Yaw = Pitch = 0;
            return rotate;
        }
    }
}