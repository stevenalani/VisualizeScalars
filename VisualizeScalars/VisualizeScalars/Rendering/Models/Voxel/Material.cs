using OpenTK;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public struct Material : IVolumeData
    {
        public Vector4 Color;
        public Vector4 ColorMapping => new Vector4(Color);

        public bool IsSetVoxel()
        {
            return Color.W != 0;
        }

        public float Density => Color.W;
    }
}