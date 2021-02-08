using System;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using OpenTK;
using VisualizeScalars.DataQuery;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models;
using VisualizeScalars.Rendering.Models.Voxel;

namespace VisualizeScalars
{
    public partial class Form1 : Form
    {
        private readonly HgtFileReader Reader;
        private readonly EarthdataDownloader downloader;
        internal static VisualizationModel<BaseGridCell> Model;
        private readonly DataFileQuerent Querernt;
        private RenderingForm renderingForm;
        private RegionSelectionForm selectionForm;
        private DetailForm detailForm;
        private DataGrid<GridCell> BaseDataGrid { get; set; }

        private readonly string workdir = "D:\\earthdata";


        public Form1()
        {
            downloader = new EarthdataDownloader(workdir, "alani", "Ibm4037!ajs");
            Reader = new HgtFileReader("D:\\earthdata");
            Querernt = new DataFileQuerent(workdir, downloader, Reader);
            InitializeComponent();
        }


        private void Form1_Load_1(object sender, EventArgs e)
        {
            selectionForm = new RegionSelectionForm(Querernt);
            renderingForm = new RenderingForm();
            renderingForm.OnClickAction = (direction) =>
            {
                var map = renderingForm.GetModel("map").FirstOrDefault();
                var intersection =
                    MathHelpers.GetIntersection2(direction, renderingForm.Camera.Position, map.Position, Vector3.UnitY);
                var gridModel = renderingForm.GetModel("model").First();
                var mat = gridModel.Modelmatrix.Inverted();
                var model2 = Vector3.TransformPosition(intersection, mat);
                var x = (int)model2.X;
                var y = (int)model2.Z;
                var values = Model.DataGrid[x, y];

                if (renderingForm.Pin == null)
                {
                    var volume = new ColorVolume<Material>(3, 10, 3, 10);
                    Material material = new Material() { Color = new Vector4(1f, 0f, 0f, 1f) };
                    for (int zv = 0; zv < 4; zv++)
                    {
                        for (int yv = 6; yv < 10; yv++)
                        {
                            for (int xv = 0; xv < 4; xv++)
                            {
                                volume.SetVoxel(xv, yv, zv, material);
                            }
                        }
                    }
                    material = new Material() { Color = new Vector4(.5f, 0.5f, 0.5f, 1f) };
                    for (int yv = 0; yv < 6; yv++)
                    {
                        volume.SetVoxel(1, yv, 1, material);
                    }
                    var mesh2 = MeshExtractor.ComputeCubicMesh(volume);
                    renderingForm.Pin = new RenderObject<PositionColorNormalVertex>(mesh2, "Pin");
                    renderingForm.Pin.PivotPoint = (volume.Dimensions.Vector3 * volume.CubeScale) / 2f;
                    renderingForm.Pin.PivotPoint.Y = 0;

                }

                renderingForm.Pin.Position = intersection;
                var details = $"Latitude: {Form1.Model.DataGrid.North - y * Form1.Model.DataGrid.GridCellsize}"+ Environment.NewLine +
                              $"Longitude: {Form1.Model.DataGrid.East + x * Form1.Model.DataGrid.GridCellsize}" +
                              Environment.NewLine;

                if (values != null)
                    foreach (var propertyName in values.Scalars)
                    {
                        details += $"{propertyName.Key,-20}:{propertyName.Value,5}" + Environment.NewLine;
                    }

                renderingForm.ttpPosDetail.SetToolTip(renderingForm.glControl, details);
                detailForm?.Close();
                detailForm = new DetailForm(values);
                detailForm.Show();
            };
            renderingForm.OnDoubleClickAction = (direction) =>
            {
                var map = renderingForm.GetModel("map").FirstOrDefault();
                var intersection =
                    MathHelpers.GetIntersection2(direction, renderingForm.Camera.Position, map.Position, Vector3.UnitY);
                var gridModel = renderingForm.GetModel("model").First();
                var mat = gridModel.Modelmatrix.Inverted();
                var model2 = Vector3.TransformPosition(intersection, mat);
                var x = (int)model2.X;
                var y = (int)model2.Z;
                var values = Model.DataGrid[x, y];
                if(values == null) return;
                renderingForm.Camera.Position = new Vector3(model2.X, values.Value(Model.HeightMapping),model2.Z);
                renderingForm.Camera.ViewDirection = -Vector3.UnitY;
                renderingForm.Camera.Yaw = 0.0;
                renderingForm.Camera.Pitch = 0.0;
                renderingForm.Camera.Update();
            };

            mtbxInterpolation.Text = "1";
            cbxSmoothing.SelectedIndex = 5;
            cbxMeshMode.SelectedIndex = 0;
            selectionForm.Show(this);
            renderingForm.Show(this);
        }

        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            clbScalarselection.Items.Clear();
            BaseDataGrid = selectionForm.GetDataGrid();
            if (BaseDataGrid == null) return;
            tbxSouth.Text = BaseDataGrid.South.ToString(CultureInfo.InvariantCulture);
            tbxWest.Text = BaseDataGrid.West.ToString(CultureInfo.InvariantCulture);
            tbxNorth.Text = BaseDataGrid.North.ToString(CultureInfo.InvariantCulture);
            tbxEast.Text = BaseDataGrid.East.ToString(CultureInfo.InvariantCulture);
           
            
            clbScalarselection.Items.AddRange(BaseDataGrid.PropertyNames);
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (BaseDataGrid == null) return;
            var items = clbScalarselection.CheckedItems.Cast<string>().ToList();
            Model = new VisualizationModel<BaseGridCell>(BaseDataGrid.Select<BaseGridCell>(items.ToArray()));
            populateTextures();
        }

        private void populateTextures()
        {
            var properties = Model.DataGrid.PropertyNames;
            cbxScalarYMapping.Items.Clear();
            cbxScalarYMapping.Items.AddRange(properties);
            cbxScalarYMapping.SelectedIndex = 0;
            flpTextures.Controls.Clear();
        }

        private void cmdLoadMap_Click(object sender, EventArgs e)
        {
            if (Model == null) return;
            var interpol = Convert.ToSingle(mtbxInterpolation.Text);
            if (Math.Abs(interpol - 1.0f) > 0.01)
            {
                Model.DataGrid.Scale(interpol, false, true);
                populateTextures();
            }

            var renderobj =
                new RenderObject<PositionNormalVertex>("model", true, Model.DataGrid.PropertyNames.Length);
            foreach (var control in flpTextures.Controls)
            {
                if (control is CustomTextureUC)
                {
                    var ctuc = control as CustomTextureUC;
                    if(ctuc.Grid == null)
                        continue;
                    renderobj.Images.Add(ctuc.Grid.CreateBitmap(ctuc.Colors,ctuc.Radius, !ctuc.colorSelection1.cbUseScalarValues.Checked));
                }
                
            }
            renderobj.Instances = renderobj.Images.Count + 1;
            UpdateMesh(renderobj);

            renderobj.PivotPoint = new Vector3(Model.DataGrid.Width/2.0f, 0, Model.DataGrid.Height / 2.0f);
            selectionForm.Activate();
            try
            {
                selectionForm.Invoke(selectionForm.getImageDelegate, 1);
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
                return;
            }
            
            //renderobj.Images.Add(selectionForm.TargetBitmap);
            var map = new ImagePlane(selectionForm.TargetBitmap);
            
            
            var coord = new ColorVolume<Material>(100,100,100);
            for (var i = 0; i < 100; i++)
            {
                coord.SetVoxel(i, 0, 0, new Material {Color = new Vector4(1, 0, 0, 1)});
                coord.SetVoxel(0, i, 0, new Material {Color = new Vector4(0, 1, 0, 1)});
                coord.SetVoxel(0, 0, i, new Material {Color = new Vector4(0, 0, 1, 1)});
            }

            var mesh2 = MeshExtractor.ComputeCubicMesh(coord);
            var renderobj2 = new RenderObject<PositionColorNormalVertex>(mesh2, "coordinates");
            

            renderingForm.ClearModels();
            var interpolation = Convert.ToSingle(mtbxInterpolation.Text);
            renderobj.Scales = new Vector3(30f / interpolation, 1f, 30f / interpolation);
            map.Position.Y = -0.01f;
            map.Scales = new Vector3(Model.DataGrid.Width * renderobj.Scales.X / map.Width,1, Model.DataGrid.Height * renderobj.Scales.Z / map.Height);
            renderingForm.AddModel(map);
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
                Model.GenerateVolume(prop);
                mesh = MeshExtractor.ComputeCubicMesh(Model);
            }
            else if (meshMode == MeshMode.GreedyCubes)
            {
                Model.GenerateVolume(prop);
                mesh = MeshExtractor.ComputeCubicMeshGreedy(Model);
            }
            else
            {
                mesh = MeshExtractor.ComputeTRN(Model);
            }

            
            renderObject.Mesh = mesh;
            SmoothMesh(renderObject);
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
            dgvScalarTable.ColumnCount = width;

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
            SmoothMesh();
            renderingForm.Render();
        }

        private void SmoothMesh(Model model = null)
        {
            if(model == null)
                model = renderingForm.GetModel("model").FirstOrDefault();
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
                    model.Mesh = MeshSmoother.LaplacianFilter(model.Mesh, (int)smoothing);
                    model.IsReady = false;
                }
                else
                {
                    model.Mesh = MeshSmoother.HCFilter(model.Mesh, (int)smoothing);
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
                var renderObject =
                    (RenderObject<PositionNormalVertex>) renderingForm.GetModel("model").FirstOrDefault();
                if (renderObject == null)
                {
                    renderObject = new RenderObject<PositionNormalVertex>("model");
                    renderingForm.AddModel(renderObject);
                }

                renderObject.PivotPoint = -new Vector3(Model.DataGrid.Width / 2f, 0, Model.DataGrid.Height / 2f);
                UpdateMesh(renderObject);
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

        
        private void tabPage3_Click(object sender, EventArgs e)
        {
        }

        private void cmdCreateTexture_Click(object sender, EventArgs e)
        {
            if(Model?.DataGrid == null) return;
            var top = 15;
            if (flpTextures.Controls.Count > 0)
            {
                var lastControl = this.flpTextures.Controls[^1];
                top = lastControl.Top + lastControl.Height + 5;
            }
            var newCtrl = new CustomTextureUC(Model.DataGrid) {Top = top};
            this.flpTextures.Controls.Add(newCtrl);
        }
    }
}