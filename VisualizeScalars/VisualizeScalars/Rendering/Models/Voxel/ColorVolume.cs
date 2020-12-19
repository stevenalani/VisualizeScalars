using OpenTK;
using VisualizeScalars.Rendering.DataStructures;

namespace VisualizeScalars.Rendering.Models.Voxel
{
    
    
    public class ColorVolume<T> : Volume<T> where T : IVolumeData, new()
    {
        protected int voxelCount = 0;
        public ColorVolume(int dimensionX, int dimensionY, int dimensionZ, float cubeScale = 1f) : base(dimensionX+1,
            dimensionY+1, dimensionZ+1)
        {
            CubeScale = cubeScale;
        }
    }
}