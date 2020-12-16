using System;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using VisualizeScalars.DataQuery;
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
        private VisualizationDataGrid VisualizationDataGrid { get; set; }
        
        
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

            shaderManager.AddShader(new []{typeof(ColorVolume<PositionColorNormalVertex>) }, ".\\Rendering\\Shaders\\DefaultVoxelShader.vert",
                ".\\Rendering\\Shaders\\DefaultVoxelShader.frag");
            shaderManager.AddShader(new[] { typeof(VisualizationModel) }, ".\\Rendering\\Shaders\\InstancedVoxelShader.vert",
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
                shader.SetUniformVector3("lightPos", Light.LightPosition);
                shader.SetUniformVector3("lightColor", Light.LightColor);
                shader.SetUniformVector3("viewpos", camera.Position);
                shader.SetUniformFloat("ambientStrength", 0.5f);
                shader.SetUniformFloat("diffuseStrength", 1f);
                shader.SetUniformFloat("specularStrength", 1.0f);
                model.Draw(shader);
            }

            glControl.SwapBuffers();
            
        }

        public LightSource Light { get; set; } = new LightSource(new OpenTK.Vector3(250,500,250),new OpenTK.Vector3(1f,1f,1f));

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
            VisualizationDataGrid = new VisualizationDataGrid(dataSet, gridsize, latSouth, lngWest);
        }
        private void cmdLoadMap_Click(object sender, EventArgs e)
        {
            
            MeshMode meshMode;
            if (cbxMeshMode.SelectedIndex == 0)
            {
                meshMode = MeshMode.MarchingCubes;
            }else if(cbxMeshMode.SelectedIndex == 1)

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
            Smoothing smoothing;
            if (cbxSmoothing.SelectedIndex == 0)
            {
                smoothing = Smoothing.None;
            }else if (cbxSmoothing.SelectedIndex == 1)
            {
                smoothing = Smoothing.Laplacian1;
            }else if (cbxSmoothing.SelectedIndex ==2)
            {
                smoothing = Smoothing.Laplacian2;
            }
            else if (cbxSmoothing.SelectedIndex ==3)
            {
                smoothing = Smoothing.Laplacian5;
            }
            else if (cbxSmoothing.SelectedIndex ==4)
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

            int interpol = Convert.ToInt32(mtbxInterpolation.Text);
            if (VisualizationDataGrid == null)
            {
                
                btnUpdateData_Click(sender, e);
            }
            modelManager.ClearModels();
            var model = new VisualizationModel(VisualizationDataGrid,interpol);
            modelManager.AddModel(model);
            //mapgen = new Mapgenerator(heights);
            /*var vol = mapgen.GenerateMapFromHeightData(interpol);

            modelManager.AddModel(vol);
            //var texVol = mapgen.GenerateTerrain(heights);
            //modelManager.AddModel(texVol);*/
            model.MeshMode = meshMode;
            model.Smoothing = smoothing;
            camera.ViewDirection = (model.Position - camera.Position).Normalized();
            mtbxInterpolation.Text = "1";
            glControl.Invalidate();
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
    }
}