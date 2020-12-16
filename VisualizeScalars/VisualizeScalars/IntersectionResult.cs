using OpenTK;
using SoilSpot.Rendering.Models;

namespace SASVoxelEngine
{
    public class IntersectionResult
    {
        public IntersectionResult(Surface surface, Vector3 intersection, Vector3 up)
        {
            Surface = surface;
            Intersection = intersection;
            var axis = Vector3.Cross(Surface.Normal, up);
            Axis = axis == Vector3.Zero ? Vector3.UnitY : axis;
        }

        public Surface Surface { get; }
        public Vector3 Intersection { get; }
        public Vector3 Axis { get; }
    }
}