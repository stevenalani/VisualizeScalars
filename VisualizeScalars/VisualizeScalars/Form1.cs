using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using VisualizeScalars.DataQuery;
using VisualizeScalars.Rendering;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models;
using VisualizeScalars.Rendering.Models.Voxel;
using PixelFormat = OpenTK.Graphics.OpenGL.PixelFormat;

namespace VisualizeScalars
{
    public partial class Form1 : Form
    {
        private readonly HgtFileReader Reader;
        private readonly EarthdataDownloader downloader;
        private VisualizationModel<BaseGridCell> Model;
        private readonly DataFileQuerent Querernt;
        private RenderingForm renderingForm;
        private RegionSelectionForm selectionForm;


        private readonly string workdir = "D:\\earthdata";


        public Form1()
        {
            downloader = new EarthdataDownloader(workdir, "alani", "Ibm4037!ajs");
            Reader = new HgtFileReader("D:\\earthdata");
            Querernt = new DataFileQuerent(workdir, downloader, Reader);


            InitializeComponent();
        }

        private DataGrid<GridCell> BaseDataGrid { get; set; }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            selectionForm = new RegionSelectionForm(Querernt);
            renderingForm = new RenderingForm();
            tbxSouth.Text = "48.75";
            tbxWest.Text = "9.1566";
            tbxNorth.Text = "48.8"; //"48.7853";
            tbxEast.Text = "9.2"; //"9.179";//"6,4";//"9.18";
            mtbxInterpolation.Text = "1";
            cbxSmoothing.SelectedIndex = 2;
            cbxMeshMode.SelectedIndex = 0;
            selectionForm.Show(this);
            renderingForm.Show(this);
        }

        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            var inputS = tbxSouth.Text.Trim().Replace('.', ',');
            var inputW = tbxWest.Text.Trim().Replace('.', ',');
            
            var inputN = tbxNorth.Text.Trim().Replace('.', ',');
            
            var inputE = tbxEast.Text.Trim().Replace('.', ',');
            
            var tstS = Convert.ToDouble(inputS);
            var tstW = Convert.ToDouble(inputW);
            var tstN = Convert.ToDouble(inputN);
            var tstE = Convert.ToDouble(inputE);
            var latSouth = tstS;
            var lngWest = tstW;
            var latNorth = tstN;
            var lngEast = tstE;
            /*
            var dataSet = Querernt.GetDataForArea(latSouth, lngWest, latNorth, lngEast);
            var gridsize = Querernt.GetGridUnitStep();
            BaseDataGrid = new DataGrid<GridCell>(dataSet, gridsize, latSouth, lngWest);
            */

            clbScalarselection.Items.Clear();
            BaseDataGrid = selectionForm.GetDataGrid();
            if(BaseDataGrid == null) return;
            
            clbScalarselection.Items.AddRange(BaseDataGrid.PropertyNames);
        }

        private void cmdLoadMap_Click(object sender, EventArgs e)
        {
            var renderobj =
                new RenderObject<PositionNormalVertex>("model", true, Model.DataGrid.PropertyNames.Length);
            UpdateMesh(renderobj);

            renderobj.PivotPoint = new Vector3(Model.DataGrid.Width, 0, Model.DataGrid.Height);
            selectionForm.Invoke(selectionForm.getImageDelegate, 1);
            //renderobj.Images.Add(selectionForm.TargetBitmap);
            var map = new ImagePlane(selectionForm.TargetBitmap);

            var coord = new ColorVolume<Material>(5, 5, 5);
            for (var i = 0; i < 5; i++)
            {
                coord.SetVoxel(i, 0, 0, new Material {Color = new Vector4(1, 0, 0, 1)});
                coord.SetVoxel(0, i, 0, new Material {Color = new Vector4(0, 1, 0, 1)});
                coord.SetVoxel(0, 0, i, new Material {Color = new Vector4(0, 0, 1, 1)});
            }

            var mesh2 = MeshExtractor.ComputeCubicMesh(coord);
            var renderobj2 = new RenderObject<PositionColorNormalVertex>(mesh2, "coordinates");
            renderobj2.Scales = Vector3.One * 10;

            renderingForm.ClearModels();
            renderingForm.AddModel(renderobj);
            renderingForm.AddModel(renderobj2);

            renderingForm.Camera.ViewDirection = (renderobj.Position - renderingForm.Camera.Position).Normalized();
            mtbxInterpolation.Text = "1.0";
            renderingForm.Render();
        }

        private void UpdateMesh(Model renderObject)
        {
            Mesh mesh;
            var prop = "Height";
            if (cbxScalarYMapping.SelectedIndex != -1)
                prop = cbxScalarYMapping.Text;
            Model.HeightMapping = prop;
            var meshMode = getMeshMode();

            if (meshMode == MeshMode.MarchingCubes)
            {
                Model.GenerateVolume(prop);
                //var min = Model.DataGrid.Min(prop)-1;
                //var max = Model.DataGrid.Max(prop);
                var isolevel = 0.1f;
                mesh = MeshExtractor.ComputeMarchingCubesMesh(Model,
                    cell => cell.IsSet ? 1.0f : 0.0f,
                    isolevel);
            }
            else if (meshMode == MeshMode.Cubes)
            {
                mesh = MeshExtractor.ComputeCubicMesh(Model);
            }
            else if (meshMode == MeshMode.GreedyCubes)
            {
                mesh = MeshExtractor.ComputeCubicMeshGreedy(Model);
            }
            else
            {
                var heights = Model.DataGrid.GetDataGrid(Model.HeightMapping);
                mesh = new GridSurface(heights, Model.DataGrid.Width, Model.DataGrid.Height);
            }

            renderObject.Mesh = mesh;
            renderObject.PivotPoint = new Vector3(Model.DataGrid.Width / 2f, 0, Model.DataGrid.Height / 2f);
            renderObject.IsReady = false;
        }

        private MeshMode getMeshMode()
        {
            MeshMode meshMode;
            if (cbxMeshMode.SelectedIndex == 1)
                meshMode = MeshMode.MarchingCubes;
            else if (cbxMeshMode.SelectedIndex == 2)
                meshMode = MeshMode.Cubes;
            else if (cbxMeshMode.SelectedIndex == 3)
                meshMode = MeshMode.GreedyCubes;
            else
                meshMode = MeshMode.GridMesh;

            return meshMode;
        }

        private Smoothing getSmoothing()
        {
            Smoothing smoothing;
            if (cbxSmoothing.SelectedIndex == 0)
                smoothing = Smoothing.None;
            else if (cbxSmoothing.SelectedIndex == 1)
                smoothing = Smoothing.Laplacian1;
            else if (cbxSmoothing.SelectedIndex == 2)
                smoothing = Smoothing.Laplacian2;
            else if (cbxSmoothing.SelectedIndex == 3)
                smoothing = Smoothing.Laplacian5;
            else if (cbxSmoothing.SelectedIndex == 4)
                smoothing = Smoothing.Laplacian10;
            else if (cbxSmoothing.SelectedIndex == 5)
                smoothing = Smoothing.LaplacianHc1;
            else if (cbxSmoothing.SelectedIndex == 6)
                smoothing = Smoothing.LaplacianHc2;
            else if (cbxSmoothing.SelectedIndex == 7)
                smoothing = Smoothing.LaplacianHc5;
            else if (cbxSmoothing.SelectedIndex == 8)
                smoothing = Smoothing.Laplacian10;
            else
                smoothing = Smoothing.Laplacian2;

            return smoothing;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbxViewScalar.SelectedIndex == -1)
                return;
            dgvScalarTable.Rows.Clear();
            dgvScalarTable.Columns.Clear();
            double unitStep, startlat, startlng;
            float[,] grid;
            if (Model != null)
            {
                grid = Model.DataGrid.GetDataGrid(cbxViewScalar.Text);
                unitStep = Model.DataGrid.GridCellsize;
                startlat = Model.DataGrid.South;
                startlng = Model.DataGrid.West;
            }
            else
            {
                grid = BaseDataGrid.GetDataGrid(cbxViewScalar.Text);
                ;
                unitStep = BaseDataGrid.GridCellsize;
                startlat = BaseDataGrid.South;
                startlng = BaseDataGrid.West;
            }

            var width = grid.GetLength(0);
            var height = grid.GetLength(1);
            dgvScalarTable.ColumnCount = height;

            for (var y = 0; y < height; y++)
            {
                var row = new DataGridViewRow();
                row.CreateCells(dgvScalarTable);
                for (var x = 0; x < width; x++)
                {
                    row.Cells[x].ToolTipText = $"Latitude:  {(y * unitStep + startlat).ToString()} \n " +
                                               $"Longitude: {(x * unitStep + startlng).ToString()}";
                    row.Cells[x].Value = grid[x, y];
                }

                dgvScalarTable.Rows.Add(row);
            }
        }

        private void cbxSmoothing_SelectedIndexChanged(object sender, EventArgs e)
        {
            var model = renderingForm.GetModel("model").FirstOrDefault();
            if (model == null) return;
            var smoothing = getSmoothing();
            if (smoothing == Smoothing.Laplacian1 ||
                smoothing == Smoothing.Laplacian2 ||
                smoothing == Smoothing.Laplacian5 ||
                smoothing == Smoothing.Laplacian10 ||
                smoothing == Smoothing.LaplacianHc1 ||
                smoothing == Smoothing.LaplacianHc2 ||
                smoothing == Smoothing.LaplacianHc5 ||
                smoothing == Smoothing.LaplacianHc10)
            {
                if (smoothing == Smoothing.Laplacian1 ||
                    smoothing == Smoothing.Laplacian2 ||
                    smoothing == Smoothing.Laplacian5 ||
                    smoothing == Smoothing.Laplacian10)
                {
                    model.Mesh = MeshSmoother.LaplacianFilter(model.Mesh, (int) smoothing);
                    model.IsReady = false;
                }
                else
                {
                    model.Mesh = MeshSmoother.HCFilter(model.Mesh, (int) smoothing);
                    model.IsReady = false;
                }
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            var model = renderingForm.GetModel("model").FirstOrDefault();
            if (model == null) return;
            if (checkBox1.Checked)
            {
                var interpolation = Convert.ToSingle(mtbxInterpolation.Text);
                model.Scales = new Vector3(30f / interpolation, 1f, 30f / interpolation);
            }
            else
            {
                model.Scales = Vector3.One;
            }

            renderingForm.Render();
        }


        private void cbxScalarYMapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Model == null) return;
            try
            {
                var mapping = ((ComboBox) sender).Text;

                var renderObject =
                    (RenderObject<PositionNormalTexcoordVertex>) renderingForm.GetModel("model").FirstOrDefault();
                if (renderObject == null)
                {
                    renderObject = new RenderObject<PositionNormalTexcoordVertex>("model");
                    renderingForm.AddModel(renderObject);
                }

                renderObject.PivotPoint = -new Vector3(Model.DataGrid.Width / 2f, 0, Model.DataGrid.Height / 2f);
                UpdateMesh(renderObject);
                var buffers = Model.GetBuffers();
                renderObject.DrawInstanced = buffers.Length > 0;
                renderObject.Instances = buffers.Length + 1;
                renderObject.Buffers.Clear();
                renderObject.Buffers.AddRange(buffers);
            }
            catch (Exception exception)
            {
                ((ComboBox) sender).SelectedIndex = -1;
                Console.WriteLine(exception);
            }
        }

        private void cbxViewScalar_Click(object sender, EventArgs e)
        {
            if (Model == null) return;

            cbxViewScalar.Items.Clear();
            cbxViewScalar.Items.AddRange(Model.DataGrid.PropertyNames);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (BaseDataGrid == null) return;
            var items = clbScalarselection.CheckedItems.Cast<string>().ToList();
            Model = new VisualizationModel<BaseGridCell>(BaseDataGrid.Select<BaseGridCell>(items.ToArray()));
            cbxScalarYMapping.Items.Clear();
            cbxScalarYMapping.Items.AddRange(Model.DataGrid.PropertyNames);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var interpol = Convert.ToSingle(mtbxInterpolation.Text);
            if (Math.Abs(interpol - 1.0f) < 0.01) return;
            Model.DataGrid.Scale(interpol, false, true);
            var model = (RenderObject<PositionNormalVertex>) renderingForm.GetModel("model").FirstOrDefault();
            if (model == null)
            {
                model = new RenderObject<PositionNormalVertex>("model");
                renderingForm.AddModel(model);
            }

            UpdateMesh(model);
        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {
            renderingForm.scalarRadius = trackBar3.Value;
        }


        private void tabPage3_Click(object sender, EventArgs e)
        {
        }
    }

    internal class ImagePlane : RenderObject<PositionNormalTexcoordVertex>
    {
        private readonly int height;
        private readonly byte[] ImageData;
        private int TexID;
        private readonly int width;

        public ImagePlane(Bitmap bitmap) : base("map")
        {
            width = bitmap.Width;
            height = bitmap.Height;
            Vertices = new[]
            {
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f, 0.0f, 0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, 0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 1.0f)
                },

                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 0.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(bitmap.Width, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(1.0f, 1.0f)
                },
                new PositionNormalTexcoordVertex
                {
                    Position = new Vector3(0.0f, 0.0f, bitmap.Height),
                    Normal = Vector3.UnitY,
                    TexCoord = new Vector2(0.0f, 1.0f)
                }
            };
            Indices = new[] {1,2,3,4,5,6};
            using var stream = new MemoryStream();
            bitmap.Save(stream, ImageFormat.Png);
            ImageData = stream.ToArray();
        }

        public override void InitBuffers()
        {
            base.InitBuffers();
            TexID = GL.GenTexture();
            GL.ActiveTexture(TextureUnit.Texture0);
            GL.BindTexture(TextureTarget.Texture2D, TexID);
            GL.TexStorage2D(TextureTarget2d.Texture2D, 1, SizedInternalFormat.Rgba32f, width, height);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int) TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapR,
                (int) TextureWrapMode.ClampToEdge);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, width, height, 0, PixelFormat.Rgba,
                PixelType.UnsignedByte, ImageData);
        }
    }
}