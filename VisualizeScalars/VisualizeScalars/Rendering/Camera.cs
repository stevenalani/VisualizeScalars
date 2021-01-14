using System;
using OpenTK;

namespace VisualizeScalars.Rendering
{
    public enum PROJECTIONTYPE
    {
        Perspective,
        Orthogonal
    }

    public partial class Camera
    {
        public const float SPEED = 10f;
        public const float SENSITIVITY = 0.1f;
        public const float ZOOM = 45.0f;

        private readonly float fov = 45;
        private PROJECTIONTYPE _projectionType;

        public EventHandler<CameraMovedEventArgs> CameraMoved = (param1, param2) => { };

        public Camera()
        {
            ClippingPlaneNear = 0.1f;
            ClippingPlaneFar = 1000f;
            Update();
        }

        public Camera(float aspectRatio, PROJECTIONTYPE projection = PROJECTIONTYPE.Perspective)
        {
            AspectRatio = aspectRatio;
            ClippingPlaneNear = 0.1f;
            ClippingPlaneFar = 100f;
            _projectionType = projection;
            Update();
            pProjection = Matrix4d.CreatePerspectiveFieldOfView((float) (fov * (Math.PI / 180)), aspectRatio,
                ClippingPlaneNear, ClippingPlaneFar);
        }

        public Camera(int width, int height, PROJECTIONTYPE projection = PROJECTIONTYPE.Perspective)
        {
            AspectRatio = width / height;
            ClippingPlaneNear = 0.1f;
            ClippingPlaneFar = 100f;
            _projectionType = projection;
            Update();
            pProjection = Matrix4d.CreatePerspectiveFieldOfView((float) (fov * (Math.PI / 180)), AspectRatio,
                ClippingPlaneNear, ClippingPlaneFar);
        }

        public Camera(int width, int height, float clippingPlaneNear, float clippingPlaneFar,
            PROJECTIONTYPE projection = PROJECTIONTYPE.Perspective)
        {
            AspectRatio = width / height;
            ClippingPlaneNear = clippingPlaneNear;
            ClippingPlaneFar = clippingPlaneFar;
            _projectionType = projection;
            Update();
            pProjection = Matrix4d.CreatePerspectiveFieldOfView((float) (fov * (Math.PI / 180)), AspectRatio,
                clippingPlaneNear, clippingPlaneFar);
        }

        private Matrix4d pProjection { get; }

        public Vector3 Position { get; set; } = new Vector3(0, 400, -1);

        public Vector3 ViewDirection { get; set; } = -Vector3.UnitZ;
        public static Vector3 Up { get; } = Vector3.UnitY;
        public Vector3 Right => Vector3.Normalize(Vector3.Cross(ViewDirection, Up));

        public float Speed { get; set; } = SPEED;

        public float Zoom { get; set; } = ZOOM;

        public float AspectRatio { get; set; }
        public float ClippingPlaneNear { get; set; }
        public float ClippingPlaneFar { get; set; }

        public Matrix4 GetView()
        {
            return Matrix4.LookAt(Position.X, Position.Y, Position.Z, Target.X, Target.Y, Target.Z, Up.X, Up.Y, Up.Z);
        }

        public Matrix4 GetProjection()
        {
            return Matrix4.CreatePerspectiveFieldOfView((float) (fov * (Math.PI / 180)), AspectRatio,
                ClippingPlaneNear, ClippingPlaneFar);
        }

        private void Update()
        {
            var rotate = GetRotationMatrix();
            ViewDirection = Vector3.Normalize(Vector3.TransformNormal(ViewDirection, rotate));
            //OnCameraMoved(new CameraMovedEventArgs() { Orientation = ViewDirection, Orgin = Position, ViewMatrix = GetView() });
        }

        public void ProcessKeyboard(CameraMovement direction, float deltaTime)
        {
            switch (direction)
            {
                case CameraMovement.FORWARD:
                    Position += ViewDirection * deltaTime * Speed;
                    break;
                case CameraMovement.RIGHT:
                    Position += Vector3.Cross(ViewDirection, Up) * deltaTime * Speed;
                    break;
                case CameraMovement.BACKWARD:
                    Position -= ViewDirection * deltaTime * Speed;
                    break;
                case CameraMovement.LEFT:
                    Position -= Vector3.Cross(ViewDirection, Up) * deltaTime * Speed;
                    break;
                case CameraMovement.UP:
                    Position += Up * deltaTime * Speed;
                    break;
                case CameraMovement.DOWN:
                    Position -= Up * deltaTime * Speed;
                    break;
            }

            Update();
        }

        public void ProcessMouseMovement(float xoffset, float yoffset)
        {
            Yaw -= xoffset * Sensitivity;
            Pitch -= yoffset * Sensitivity * ViewDirection.Z / Math.Abs(ViewDirection.Z);

            Update();
        }

        public void ProcessMouseScroll(float yoffset)
        {
            if (Zoom >= 1.0f && Zoom <= 45.0f)
                Zoom -= yoffset;
            if (Zoom <= 1.0f)
                Zoom = 1.0f;
            if (Zoom >= 45.0f)
                Zoom = 45.0f;
        }

        protected virtual void OnCameraMoved(CameraMovedEventArgs e)
        {
            var handler = CameraMoved;
            if (handler != null) handler(this, e);
        }
    }


    public class CameraMovedEventArgs : EventArgs
    {
        public Vector3 Orgin;
        public Vector3 Orientation;
        public Matrix4 ViewMatrix;
    }
}