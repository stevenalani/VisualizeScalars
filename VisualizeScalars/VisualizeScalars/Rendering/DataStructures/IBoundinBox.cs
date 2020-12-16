using VisualizeScalars.Rendering.Models;

namespace VisualizeScalars.Rendering.DataStructures
{
    public interface IBoundinBox
    {
        BoundingBox BoundingBox { get; set; }
    }
}