using System;
using System.Linq;
using System.Windows.Forms;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using OpenTK;
using OpenTK.Graphics.OpenGL;
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
        
        private readonly ShaderManager shaderManager;
        private readonly ModelManager modelManager;
        
        private Camera camera = new Camera();
        private DateTime nextWireframeSwitch = DateTime.Now;
        private bool IsWireframe = false;

        public Vector2 LastMousePosition { get; private set; }
        private bool firstMouseMove = true;
        private string workdir = "D:\\earthdata";
        private readonly HgtFileReader Reader;
        EarthdataDownloader downloader;
        DataFileQuerent Querernt;

        private DataGrid<GridCell> BaseDataGrid { get; set; }
        private VisualizationModel<BaseGridCell> Model;
        private float scalarRadius = 2.5f;
        private RegionSelectionForm selectionForm;

        public LightSource Light { get; set; } = new LightSource(new OpenTK.Vector3(0, 100, -1), new OpenTK.Vector3(1f, 1f, 1f));
        public float AmbientStrength { get; set; } = 0.2f;
        public float DiffuseStrength { get; set; } = 0.5f;
        public float SpecularStrength { get; set; } = 1f;

        public Form1()
        {
            downloader = new EarthdataDownloader(workdir, "alani", "Ibm4037!ajs");
            Reader = new HgtFileReader("D:\\earthdata");
            Querernt = new  DataFileQuerent(workdir,downloader,Reader);
            modelManager = new ModelManager();
            shaderManager = new ShaderManager();

            shaderManager.AddShader(new []{typeof(ColorVolume<Material>),typeof(RenderObject<PositionColorNormalVertex>) }, ".\\Rendering\\Shaders\\DefaultVoxelShader.vert",
                ".\\Rendering\\Shaders\\DefaultVoxelShader.frag");
            shaderManager.AddShader(new[] { typeof(RenderObject<PositionNormalVertex>) }, ".\\Rendering\\Shaders\\InstancedVoxelShader.vert",
                ".\\Rendering\\Shaders\\InstancedVoxelShader.frag");
            
            shaderManager.AddShader(typeof(TextureVolume), ".\\Rendering\\Shaders\\VolumetricRenderingShader.vert",
                ".\\Rendering\\Shaders\\VolumetricRenderingShader.frag");
            
            InitializeComponent();
        }

        
        private void Form1_Load_1(object sender, EventArgs e)
        {
            selectionForm = new RegionSelectionForm(Querernt);
            tbxSouth.Text = "48.75";
            tbxWest.Text = "9.1566";
            tbxNorth.Text = "48.8";//"48.7853";
            tbxEast.Text = "9.2";//"9.179";//"6,4";//"9.18";
            mtbxInterpolation.Text = "1";
            cbxSmoothing.SelectedIndex = 2;
            cbxMeshMode.SelectedIndex = 0;
            this.selectionForm.Show(this);
        }
       
        private void glControl_Resize(object sender, EventArgs e)
        {
            var control = (GLControl) sender;
            GL.Viewport(0, 0, control.Width, control.Height);
            camera.AspectRatio = control.AspectRatio;
        }
        private void btnUpdateData_Click(object sender, EventArgs e)
        {
            string inputS = tbxSouth.Text.Trim().Replace('.', ',');
            string inputW = tbxWest.Text.Trim().Replace('.', ','); ;
            string inputN = tbxNorth.Text.Trim().Replace('.', ','); ;
            string inputE = tbxEast.Text.Trim().Replace('.', ','); ;
            var tstS = Convert.ToDouble(inputS);
            var tstW = Convert.ToDouble(inputW);
            var tstN = Convert.ToDouble(inputN);
            var tstE = Convert.ToDouble(inputE);
            double latSouth = tstS;
            double lngWest = tstW;
            double latNorth = tstN;
            double lngEast = tstE;
            /*
            var dataSet = Querernt.GetDataForArea(latSouth, lngWest, latNorth, lngEast);
            var gridsize = Querernt.GetGridUnitStep();
            BaseDataGrid = new DataGrid<GridCell>(dataSet, gridsize, latSouth, lngWest);
            */
            BaseDataGrid = selectionForm.GetDataGrid();
            
            clbScalarselection.Items.Clear();
            clbScalarselection.Items.AddRange(BaseDataGrid.PropertyNames);


        }
        private void cmdLoadMap_Click(object sender, EventArgs e)
        {
           

            modelManager.ClearModels();
            DataGrid<BaseGridCell> grid = Model.DataGrid;
            
            var renderobj = new RenderObject<PositionNormalVertex>("model",true,Model.DataGrid.PropertyNames.Length);
            UpdateMesh(renderobj);

            renderobj.PivotPoint = Model.Dimensions.Vector3 / 2;
            var image = selectionForm.GetImageData();
            //renderobj.Buffers.Add(new BufferStorage(Enumerable.Cast<GridCell>()));
            modelManager.AddModel(renderobj);
            var coord = new ColorVolume<Material>(5,5,5);
            for (int i = 0; i < 5; i++)
            {
                coord.SetVoxel(i,0,0,new Material(){Color = new Vector4(1,0,0,1)});
                coord.SetVoxel(0,i,0,new Material(){Color = new Vector4(0,1,0,1)});
                coord.SetVoxel(0,0,i,new Material(){Color = new Vector4(0,0,1,1)});
            }
            var mesh2 = MeshExtractor.ComputeCubicMesh(coord);
            var renderobj2 = new RenderObject<PositionColorNormalVertex>(mesh2,"coordinates",false);
            renderobj2.Scales = Vector3.One * 10;
            modelManager.AddModel(renderobj2);
            camera.ViewDirection = (renderobj.Position - camera.Position).Normalized();
            mtbxInterpolation.Text = "1";
            glControl.Invalidate();
            
        }

        private void UpdateMesh(Model renderObject)
        {
            Mesh mesh;
            string prop = "Height";
            if (cbxScalarYMapping.SelectedIndex != -1)
                prop = cbxScalarYMapping.Text;
            Model.HeightMapping = prop;
            MeshMode meshMode = getMeshMode();

            if (meshMode == MeshMode.MarchingCubes)
            {
                Model.GenerateVolume(prop);
                var min = Model.DataGrid.Min(prop)-1;
                var max = Model.DataGrid.Max(prop);
                float isolevel = 0.1f;
                mesh = MeshExtractor.ComputeMarchingCubesMesh(Model,
                    cell => (float)(cell.IsSet?1.0f:0.0f),
                    isolevel);
            }
            else if (meshMode == MeshMode.Cubes)
            {
                mesh = MeshExtractor.ComputeCubicMesh(Model);
            }
            else if(meshMode == MeshMode.GreedyCubes)
            {
                mesh = MeshExtractor.ComputeCubicMeshGreedy(Model);
            }
            else
            {
                var heights = Model.DataGrid.GetDataGrid(Model.HeightMapping);
                mesh = new GridSurface(heights,Model.DataGrid.Width,Model.DataGrid.Height);
            }
            
            renderObject.Mesh = mesh;
            renderObject.PivotPoint = -new Vector3(Model.DataGrid.Width / 2f, 0, Model.DataGrid.Height / 2f);

            renderObject.IsReady = false;
        }
        MeshMode getMeshMode()
        {
            MeshMode meshMode;
            if (cbxMeshMode.SelectedIndex == 1)
            {
                meshMode = MeshMode.MarchingCubes;
            }
            else if (cbxMeshMode.SelectedIndex == 2)

            {
                meshMode = MeshMode.Cubes;
            }
            else if (cbxMeshMode.SelectedIndex == 3)
            {
                meshMode = MeshMode.GreedyCubes;
            }
            else
            {
                meshMode = MeshMode.GridMesh;
            }

            return meshMode;
        }
        Smoothing getSmoothing()
        {
            Smoothing smoothing;
            if (cbxSmoothing.SelectedIndex == 0)
            {
                smoothing = Smoothing.None;
            }
            else if (cbxSmoothing.SelectedIndex == 1)
            {
                smoothing = Smoothing.Laplacian1;
            }
            else if (cbxSmoothing.SelectedIndex == 2)
            {
                smoothing = Smoothing.Laplacian2;
            }
            else if (cbxSmoothing.SelectedIndex == 3)
            {
                smoothing = Smoothing.Laplacian5;
            }
            else if (cbxSmoothing.SelectedIndex == 4)
            {
                smoothing = Smoothing.Laplacian10;
            }
            else if (cbxSmoothing.SelectedIndex == 5)
            {
                smoothing = Smoothing.LaplacianHc1;
            }
            else if (cbxSmoothing.SelectedIndex == 6)
            {
                smoothing = Smoothing.LaplacianHc2;
            }
            else if (cbxSmoothing.SelectedIndex == 7)
            {
                smoothing = Smoothing.LaplacianHc5;
            }
            else if (cbxSmoothing.SelectedIndex == 8)
            {
                smoothing = Smoothing.Laplacian10;
            }
            else
            {
                smoothing = Smoothing.Laplacian2;
            }

            return smoothing;
        }
        private void glControl_MouseMove(object sender, MouseEventArgs e)
        {
            var currentPosition = new Vector2(e.X, e.Y);
            if (e.Button == MouseButtons.Right)
            {
                if (firstMouseMove)
                {
                    LastMousePosition = currentPosition;
                    firstMouseMove = false;
                    return;
                }
                
                var delta = LastMousePosition - currentPosition;
                camera.ProcessMouseMovement(delta.X, delta.Y);
                glControl.Invalidate();
            }else if (e.Button == MouseButtons.Middle)
            {
               
                    if (firstMouseMove)
                    {
                        LastMousePosition = currentPosition;
                        firstMouseMove = false;
                        return;
                    }

                    var delta = LastMousePosition - currentPosition;
                    camera.Position += (Camera.Up * delta.Y);
                    camera.Position += (camera.Right * delta.X);
                    glControl.Invalidate();
                
            }

            LastMousePosition = currentPosition;
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyCode == Keys.G)
            {
                SwitchWireFrame();
            }
        }

        private void SwitchWireFrame()
        {
            if (nextWireframeSwitch < DateTime.Now)
            {
                IsWireframe = !IsWireframe;
                if (IsWireframe)
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                else
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                nextWireframeSwitch = DateTime.Now.AddSeconds(0.5);
            }
        }

        private void glControl_Scroll(object sender, ScrollEventArgs e)
        {

        }



        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            camera.Position += camera.ViewDirection * e.Delta *.25f;
            glControl.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            camera.ClippingPlaneFar = (float)trackBar1.Value;
            trackBar2.Maximum = (int)camera.ClippingPlaneFar;
            glControl.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            camera.ClippingPlaneNear = (float)trackBar2.Value/1000;
            glControl.Invalidate();
        }
        
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            Light.Position.X = tbLightX.Value;
            lLightPosX.Text = tbLightX.Value.ToString();
            glControl.Invalidate();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            Light.Position.Y = tbLightY.Value;
            lLightPosY.Text = tbLightY.Value.ToString();
            glControl.Invalidate();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            Light.Position.Z = tbLightZ.Value;
            lLightPosZ.Text = tbLightZ.Value.ToString();
            glControl.Invalidate();
        }

        private void tbSpecularFactor_Scroll(object sender, EventArgs e)
        {
            SpecularStrength = tbSpecularFactor.Value / 100f;
            glControl.Invalidate();
        }

        private void trackBar7_Scroll(object sender, EventArgs e)
        {
            AmbientStrength = tbAmbientFactor.Value / 1000f;
            glControl.Invalidate();
        }

        private void tbDiffuseFactor_Scroll(object sender, EventArgs e)
        {
            DiffuseStrength = tbDiffuseFactor.Value / 1000f;
            glControl.Invalidate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbxViewScalar.SelectedIndex == -1) 
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
                grid = BaseDataGrid.GetDataGrid(cbxViewScalar.Text); ;
                unitStep = BaseDataGrid.GridCellsize;
                startlat = BaseDataGrid.South;
                startlng = BaseDataGrid.West;
            }
            int width = grid.GetLength(0);
            int height = grid.GetLength(1);
            dgvScalarTable.ColumnCount = height;
            
            for (int y = 0; y < height; y++)
            {
                DataGridViewRow row = new DataGridViewRow();
                row.CreateCells(this.dgvScalarTable);
                for (int  x = 0; x < width; x++)
                {
                    row.Cells[x].ToolTipText = $"Latitude:  {(y * unitStep + startlat).ToString()} \n " +
                                               $"Longitude: {(x * unitStep + startlng).ToString()}";
                    row.Cells[x].Value = grid[x, y];
                }

                this.dgvScalarTable.Rows.Add(row);
            }
        }

        private void cbxSmoothing_SelectedIndexChanged(object sender, EventArgs e)
        {
            var model = modelManager.GetModel("model").FirstOrDefault();
            if (model == null) return;
            Smoothing smoothing = getSmoothing();
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
            var model = modelManager.GetModel("model").FirstOrDefault();
            if (model == null) return;
            if (checkBox1.Checked)
            {
                var interpolation = Convert.ToSingle(mtbxInterpolation.Text);
                model.Scales = new Vector3(30f/ interpolation, 1f, 30f/ interpolation);
            }
            else
            {
                model.Scales = Vector3.One;
            }
            glControl.Invalidate();
        }


        private void cbxScalarYMapping_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Model == null) return;
            try
            {
                var mapping = ((ComboBox) sender).Text;
                
                RenderObject<PositionNormalVertex> model = (RenderObject<PositionNormalVertex>) modelManager.GetModel("model").FirstOrDefault();
                if (model == null)
                {
                    model = new RenderObject<PositionNormalVertex>("model");
                    modelManager.AddModel(model);
                }

                model.PivotPoint = -new Vector3(Model.DataGrid.Width/2f,0, Model.DataGrid.Height / 2f);
                UpdateMesh(model);
                var buffers = Model.GetBuffers();
                model.DrawInstanced = buffers.Length > 0;
                model.Instances = buffers.Length + 1;
                model.Buffers.Clear();
                model.Buffers.AddRange(buffers);
            }
            catch (Exception exception)
            {
                ((ComboBox) sender).SelectedIndex = -1;
                Console.WriteLine(exception);
            }

        }
        
        private void cbxViewScalar_Click(object sender, EventArgs e)
        {
            if(Model == null) return;
            
            cbxViewScalar.Items.Clear();
            cbxViewScalar.Items.AddRange(Model.DataGrid.PropertyNames);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(BaseDataGrid == null ) return;
            var items = clbScalarselection.CheckedItems.Cast<string>().ToList();
            Model = new VisualizationModel<BaseGridCell>(BaseDataGrid.Select<BaseGridCell>(items.ToArray()));
            cbxScalarYMapping.Items.Clear();
            cbxScalarYMapping.Items.AddRange(Model.DataGrid.PropertyNames);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            float interpol = Convert.ToSingle(mtbxInterpolation.Text);
            if (Math.Abs(interpol - 1.0f) < 0.01) return;
            Model.DataGrid.Scale(interpol, normalize: false, trim: true);
           RenderObject<PositionNormalVertex> model = (RenderObject<PositionNormalVertex>)modelManager.GetModel("model").FirstOrDefault();
            if (model == null)
            {
                model = new RenderObject<PositionNormalVertex>("model");
                modelManager.AddModel(model);
            }
            UpdateMesh(model);
        }

        private void trackBar3_Scroll_1(object sender, EventArgs e)
        {
            scalarRadius = (float)trackBar3.Value;
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            camera.AspectRatio = glControl.AspectRatio;
            GL.ClearColor(0.1f, 0.1f, 0.2f, 1.0f);
            GL.Enable(EnableCap.Multisample);
            //GL.Hint(HintTarget.MultisampleFilterHintNv, HintMode.Nicest);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Front);
            GL.FrontFace(FrontFaceDirection.Ccw);
            GL.Enable(EnableCap.Blend);
            GL.BlendFunc(BlendingFactor.SrcAlpha, BlendingFactor.OneMinusSrcAlpha);
            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            shaderManager.InitPrograms();
            glControl.Invalidate();
        }

        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            var projection = camera.GetProjection();
            var view = camera.GetView();
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (modelManager.HasModelUpdates) modelManager.InitModels();


            foreach (var model in modelManager.GetModels())
            {
                if (!model.IsReady) model.InitBuffers();
                var shader = shaderManager.GetShader(model.GetType());
                shader.Use();
                shader.SetUniformMatrix4X4("projection", projection);
                shader.SetUniformMatrix4X4("view", view);
                shader.SetUniformVector3("lightPos", Light.Position);
                shader.SetUniformVector3("lightColor", Light.Color);
                shader.SetUniformVector3("viewpos", camera.Position);
                shader.SetUniformFloat("ambientStrength", AmbientStrength);
                shader.SetUniformFloat("diffuseStrength", DiffuseStrength);
                shader.SetUniformFloat("specularStrength", SpecularStrength);
                shader.SetUniformFloat("radius", scalarRadius);

                shader.SetUniformVector4("LayerColor[0]", new Vector4(0.5f, 0.5f, 0.5f, 1f));
                shader.SetUniformVector4("LayerColor[1]", new Vector4(1f, 0f, 0f, .3f));
                shader.SetUniformVector4("LayerColor[2]", new Vector4(0f, 1f, 0f, .3f));
                shader.SetUniformVector4("LayerColor[3]", new Vector4(0f, 0f, 1f, .3f));
                shader.SetUniformVector4("LayerColor[4]", new Vector4(1f, 0f, 0f, .3f));
                shader.SetUniformVector4("LayerColor[5]", new Vector4(0f, 1f, 0f, .3f));
                model.Draw(shader);
            }

            glControl.SwapBuffers();

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {
           
        }
    }
}