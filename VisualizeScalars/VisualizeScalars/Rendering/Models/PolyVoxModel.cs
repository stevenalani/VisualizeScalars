using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics;

using OpenTK;
using PolyVoxMapper;
using SixLabors.ImageSharp.PixelFormats;
using SoilSpot.Rendering.DataStructures;
using SoilSpot.Rendering.Models.Voxel;
using SoilSpot.Rendering.ShaderImporter;

using Vector3 = OpenTK.Vector3;


namespace SoilSpot.Rendering.Models
{
    public class PolyVoxModel : PositionColorNormalModel
    {
        private readonly Region currentRegion;
        public List<Material> Materials = new List<Material> {new Material()};
        private readonly RawVolume volume;

        public PolyVoxModel(int width, int height, int depth, string name = "RawModel") : base(null, null, name)
        {
            Dimensions = new Vector3Int(width, height, depth);
            
            currentRegion = new Region(0, 0, 0, width, height, depth);
            volume = new RawVolume(currentRegion);
        }

        public PolyVoxModel(PositionColorNormalVertex[] vertices, int[] indices, string modelname = "untitled") : base(
            vertices, indices, modelname)
        {
        }

        public Vector3Int Dimensions { get; set; }


        public override void Draw(ShaderProgram shaderProgram)
        {
            base.Draw(shaderProgram);
        }

        public override void InitBuffers()
        {
            var mesh = volume.extractMarchingCubesMesh();
            Vertices = mesh.vertices.Select(x =>new PositionColorNormalVertex(){ Position = new Vector3(x.position.x, x.position.y, x.position.z), Normal = new Vector3(x.normal.x,x.normal.y,x.normal.z)}).ToArray();
            Indices = mesh.indices.Select(x => (int)x).ToArray();
            base.InitBuffers();
        }

        public void SetVoxel(int x, int y, int z, Material material)
        {
            if (!Materials.Contains(material)) Materials.Add(material);

            var idx = (uint) Materials.IndexOf(material);
            volume.setVoxel(x, y, z, idx);
        }

        public void Prefetch(Vector3 eOrgin)
        {
            var mesh = volume.extractMarchingCubesMesh();
            mesh.vertices.Select(x =>
            {
                var pos = new OpenTK.Vector3(x.position.x, x.position.y, x.position.z);
                var norm = new Vector3(x.normal.x, x.normal.y, x.normal.z);
                var cal = new Rgba32();
                cal.PackedValue = (uint) x.material;
                var floatcol = cal.ToVector4();
                return new PositionColorNormalVertex
                {
                    Position = pos,
                    Normal = norm,
                    Color = Materials[(int) x.material].Color
                };
            }).ToArray();
            IsReady = false;
        }
    }
}