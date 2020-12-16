using System;
using OpenTK;

namespace SoilSpot.Rendering
{
    public partial class Camera
    {
        public const float YAW = 0f;
        public const float PITCH = 0f;
        public double Pitch = PITCH;
        public double Sensitivity = SENSITIVITY;

        public double Yaw = YAW;

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