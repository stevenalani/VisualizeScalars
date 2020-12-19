using OpenTK;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    public interface IVolumeData
    {
        public Vector4 ColorMapping { get; }
        public bool IsSetVoxel();
    }
}