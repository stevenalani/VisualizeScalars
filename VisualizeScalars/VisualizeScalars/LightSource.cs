using OpenTK;

namespace VisualizeScalars
{
    public class LightSource
    {
        public Vector3 Color;
        public Vector3 Position;

        //public Matrix4 LightView => Matrix4.CreateOrthographic()
        public LightSource(Vector3 position, Vector3 color)
        {
            Color = color;
            Position = position;
        }
    }
}