using SoilSpot.Rendering.Models;

namespace SoilSpot.Rendering.DataStructures
{
    public interface IBoundinBox
    {
        BoundingBox BoundingBox { get; set; }
    }
}