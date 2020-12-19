using System;
using System.Linq;
using System.Windows.Forms;
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
        private string workdir = "D:\\earthdata";
        private Camera camera = new Camera();
        private bool firstMouseMove = true;
        private readonly ShaderManager shaderManager;
        private readonly ModelManager modelManager;
        private readonly HgtFileReader Reader;
        EarthdataDownloader downloader;
        DataFileQuerent Querernt;
        private DataGrid<GridCell> BaseDataGrid { get; set; }
        private VisualizationModel<GridCell> Model;
        
        
        private DateTime nextWireframeSwitch = DateTime.Now;
        private bool IsWireframe = false;
        public Vector2 LastMousePosition { get; private set; }
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
            tbxSouth.Text = "48.78";
            tbxWest.Text = "9.18";
            tbxNorth.Text = "48.81";
            tbxEast.Text = "9.21";//"6,4";//"9.18";
            mtbxInterpolation.Text = "1";
            cbxSmoothing.SelectedIndex = 2;
            cbxMeshMode.SelectedIndex = 0;
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

        public LightSource Light { get; set; } = new LightSource(new OpenTK.Vector3(0,100,-1),new OpenTK.Vector3(1f,1f,1f));
        public float AmbientStrength { get; set; } = 0.2f;
        public float DiffuseStrength { get; set; } = 0.5f;
        public float SpecularStrength { get; set; } = 1f;
       
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

            var dataSet = Querernt.GetDataForArea(latSouth, lngWest, latNorth, lngEast);
            var gridsize = Querernt.GetGridUnitStep();
            
            BaseDataGrid = new DataGrid<GridCell>(dataSet, gridsize, latSouth, lngWest);
            comboBox1.Items.Clear();
            comboBox1.Items.AddRange(BaseDataGrid.PropertyNames);
            clbScalarselection.Items.Clear();
            clbScalarselection.Items.AddRange(BaseDataGrid.PropertyNames);
            //clbScalarselection.SelectionMode = SelectionMode.MultiSimple;

        }
        private void cmdLoadMap_Click(object sender, EventArgs e)
        {
            if (BaseDataGrid == null)
            {
                
                btnUpdateData_Click(sender, e);
            }
            MeshMode meshMode = getMeshMode();

            Smoothing smoothing = getSmoothing();


            float interpol = Convert.ToSingle(mtbxInterpolation.Text);

            modelManager.ClearModels();
            DataGrid<GridCell> grid = BaseDataGrid.Select(new[]{"Height", "ParticulateMatter2_5", "ParticulateMatter10","Temperature" });
            if (interpol != 1)
            {
                grid = BaseDataGrid.Scale(interpol,normalize: false);
            }
            
            Model = new VisualizationModel<GridCell>(grid);
            Model.GenerateVolume("Height");
            Mesh mesh;
            if(meshMode == MeshMode.MarchingCubes)
            {
                var min = grid.Min("Height");
                var max = grid.Max("Height");
                var isolevel = 0.0000000001f;
                mesh = MeshExtractor.ComputeMarchingCubesMesh<GridCell>(Model,
                    cell => (float)((cell.Height)/(max)),
                    isolevel);
            }
            else if (meshMode == MeshMode.Cubes)
            {
                mesh = MeshExtractor.ComputeCubicMesh<GridCell>(Model);
            }
            else
            {
                mesh = MeshExtractor.ComputeCubicMeshGreedy<GridCell>(Model);
            }
            
            var renderobj = new RenderObject<PositionNormalVertex>(mesh,"model");
            renderobj.PivotPoint = Model.Dimensions.Vector3 / 2;
            
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
            var renderobj2 = new RenderObject<PositionColorNormalVertex>(mesh2,"coordinates");
            modelManager.AddModel(renderobj2);
            camera.ViewDirection = (renderobj.Position - camera.Position).Normalized();
            mtbxInterpolation.Text = "1";
            glControl.Invalidate();

        } 
        MeshMode getMeshMode()
        {
            MeshMode meshMode;
            if (cbxMeshMode.SelectedIndex == 0)
            {
                meshMode = MeshMode.MarchingCubes;
            }
            else if (cbxMeshMode.SelectedIndex == 1)

            {
                meshMode = MeshMode.Cubes;
            }
            else if (cbxMeshMode.SelectedIndex == 2)
            {
                meshMode = MeshMode.GreedyCubes;
            }
            else
            {
                meshMode = MeshMode.MarchingCubes;
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
            glControl.Invalidate();
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            Light.Position.Y = tbLightY.Value;
            glControl.Invalidate();
        }

        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            Light.Position.Z = tbLightZ.Value;
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
            if(comboBox1.SelectedIndex == -1) 
                return;
            textBox1.Clear();
            float[,] grid;
            if (Model != null)
            {
                grid = Model.DataGrid.GetDataGrid(comboBox1.Text);
            }
            else
            {
                grid = BaseDataGrid.GetDataGrid(comboBox1.Text); ;
            }

            for (int y = 0; y < grid.GetLength(1); y++)
            {
                string output = "";
                for (int  x = 0; x < grid.GetLength(0); x++)
                {
                    output += " " + String.Format("{0,10:0.0}", grid[x,y]);
                }

                textBox1.Text += output + Environment.NewLine;
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
        }

        private void clbScalarselection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(Model == null) return;
            var items = clbScalarselection.SelectedItems;
            //Model.DataGrid = Model.DataGrid.Select();
            var model = modelManager.GetModel("model").FirstOrDefault();
            if (model == null) return;


        }
    }
}