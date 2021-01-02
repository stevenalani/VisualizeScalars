using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using VisualizeScalars.Rendering;
using VisualizeScalars.Rendering.DataStructures;
using VisualizeScalars.Rendering.Models;
using VisualizeScalars.Rendering.Models.Voxel;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars
{
    public partial class RenderingForm : Form
    {
        const int SHADOW_WIDTH = 1024, SHADOW_HEIGHT = 1024;
        private ShaderProgram ShadowShader;

        public Camera Camera = new Camera();
        private bool firstMouseMove = true;
        private bool IsWireframe;

        private DateTime nextWireframeSwitch = DateTime.Now;
        private int depthMapFBO;
        private int depthMap;
        public RenderingForm()
        {
            modelManager = new ModelManager();
            shaderManager = new ShaderManager();
            ShadowShader = new ShaderProgram(".\\Rendering\\Shaders\\shadowShader.vert",
                ".\\Rendering\\Shaders\\shadowShader.frag");
            shaderManager.AddShader(
                new[] {typeof(ColorVolume<Material>), typeof(RenderObject<PositionColorNormalVertex>)},
                ".\\Rendering\\Shaders\\DefaultVoxelShader.vert",
                ".\\Rendering\\Shaders\\DefaultVoxelShader.frag");
            shaderManager.AddShader(new[] {typeof(RenderObject<PositionNormalVertex>)},
                ".\\Rendering\\Shaders\\InstancedVoxelShader.vert",
                ".\\Rendering\\Shaders\\InstancedVoxelShader.frag");

            shaderManager.AddShader(typeof(ImagePlane), ".\\Rendering\\Shaders\\ImageShader.vert",
                ".\\Rendering\\Shaders\\ImageShader.frag");
           /* shaderManager.AddShader(typeof(TextureVolume), ".\\Rendering\\Shaders\\VolumetricRenderingShader.vert",
                ".\\Rendering\\Shaders\\ImageShader.frag");*/

            InitializeComponent();
        }

        private ModelManager modelManager { get; }
        public ShaderManager shaderManager { get; set; }
        public Vector2 LastMousePosition { get; private set; }
        public LightSource Light { get; set; } = new LightSource(new Vector3(0, 100, -1), new Vector3(1f, 1f, 1f));
        public float AmbientStrength { get; set; } = 0.2f;
        public float DiffuseStrength { get; set; } = 0.5f;
        public float SpecularStrength { get; set; } = 1f;
        public float scalarRadius { get; set; } = 2.5f;


        private void glControl_Resize(object sender, EventArgs e)
        {
            var control = (GLControl) sender;
            GL.Viewport(0, 0, control.Width, control.Height);
            Camera.AspectRatio = control.AspectRatio;
        }

        private void RenderingView_Load(object sender, EventArgs e)
        {
        }

        private void glControl_Load(object sender, EventArgs e)
        {
            Camera.AspectRatio = glControl.AspectRatio;
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
            ShadowShader.SetupShader();
            loadDepthMap();
            glControl.Invalidate();
        }

        private void loadDepthMap()
        {
            

            depthMapFBO = GL.GenFramebuffer();
            depthMap = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, depthMap);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.DepthComponent, SHADOW_WIDTH, SHADOW_HEIGHT, 0, PixelFormat.DepthComponent, PixelType.Float, IntPtr.Zero);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.ClampToBorder);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.ClampToBorder);
            float[] borderColor = { 1.0f, 1.0f, 1.0f, 1.0f };
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureBorderColor, borderColor);
            // attach depth texture as FBO's depth buffer
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            GL.FramebufferTexture2D(FramebufferTarget.Framebuffer,FramebufferAttachment.DepthAttachment,TextureTarget.Texture2D, depthMap, 0);
            GL.DrawBuffer(DrawBufferMode.None);
            GL.ReadBuffer(ReadBufferMode.None);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);


        }
        private void glControl_Paint(object sender, PaintEventArgs e)
        {
            var projection = Camera.GetProjection();
            var view = Camera.GetView();
            
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            if (modelManager.HasModelUpdates) modelManager.InitModels();
            
            GL.Viewport(0, 0, SHADOW_WIDTH, SHADOW_HEIGHT);
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, depthMapFBO);
            GL.Clear(ClearBufferMask.DepthBufferBit);
            GL.ActiveTexture(TextureUnit.Texture0);
            ShadowShader.Use();
            ShadowShader.SetUniformMatrix4X4("lightSpaceMatrix", Light.LightSpaceMatrix(glControl.AspectRatio, 1.0f, 1000f));
            foreach (var model in modelManager.GetModelsOrdered(Camera.Position))
            {
                if (!model.IsReady) model.InitBuffers();
                
                model.Draw(ShadowShader);
            }
            GL.BindFramebuffer(FramebufferTarget.Framebuffer, 0);
            GL.Viewport(0, 0, glControl.Width,glControl.Height);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.ActiveTexture(TextureUnit.Texture1);
            GL.BindTexture(TextureTarget.Texture2D, depthMap);
            foreach (var model in modelManager.GetModelsOrdered(Camera.Position))
            {
                if (!model.IsReady) model.InitBuffers();
                var shader = shaderManager.GetShader(model.GetType());
                shader.Use();

                shader.SetUniformMatrix4X4("projection", projection);
                shader.SetUniformMatrix4X4("view", view);
                shader.SetUniformMatrix4X4("lightSpaceMatrix",Light.LightSpaceMatrix(glControl.AspectRatio, 1.0f, 1000f));
                shader.SetUniformVector3("lightPos", Light.Position);
                shader.SetUniformVector3("lightColor", Light.Color);
                shader.SetUniformVector3("viewpos", Camera.Position);
                shader.SetUniformFloat("ambientStrength", AmbientStrength);
                shader.SetUniformFloat("diffuseStrength", DiffuseStrength);
                shader.SetUniformFloat("specularStrength", SpecularStrength);
                shader.SetUniformVector4("layer0Color", new Vector4(0.5f, 0.5f, 0.5f, 0.6f));
                shader.SetUniformFloat("yOffset", tbTextureOffset.Value / 10f);
                model.Draw(shader);
            }

            glControl.SwapBuffers();
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
                Camera.ProcessMouseMovement(delta.X, delta.Y);
                glControl.Invalidate();
            }
            else if (e.Button == MouseButtons.Middle)
            {
                if (firstMouseMove)
                {
                    LastMousePosition = currentPosition;
                    firstMouseMove = false;
                    return;
                }

                var delta = LastMousePosition - currentPosition;
                Camera.Position += Camera.Up * delta.Y;
                Camera.Position += Camera.Right * delta.X;
                glControl.Invalidate();
            }

            LastMousePosition = currentPosition;
        }

        private void glControl_KeyDown(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = true;
            if (e.KeyCode == Keys.G) SwitchWireFrame();
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


        private void glControl_MouseWheel(object sender, MouseEventArgs e)
        {
            Camera.Position += Camera.ViewDirection * e.Delta * .25f;
            glControl.Invalidate();
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            Camera.ClippingPlaneFar = trackBar1.Value;
            trackBar2.Maximum = (int) Camera.ClippingPlaneFar-1;
            glControl.Invalidate();
        }

        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            Camera.ClippingPlaneNear = (float) trackBar2.Value / 1000;
            glControl.Invalidate();
        }


        private void tbSpecularFactor_Scroll(object sender, EventArgs e)
        {
            SpecularStrength = tbSpecularFactor.Value / 100f;
            glControl.Invalidate();
        }


        private void tbDiffuseFactor_Scroll(object sender, EventArgs e)
        {
            DiffuseStrength = tbDiffuseFactor.Value / 1000f;
            glControl.Invalidate();
        }

        public void ClearModels()
        {
            modelManager.ClearModels();
        }

        public void AddModel(Model renderobj)
        {
            modelManager.AddModel(renderobj);
        }

        public List<Model> GetModel(string model)
        {
            return modelManager.GetModel(model);
        }

        public void Render()
        {
            glControl.Invalidate();
        }

        private void tbLightX_Scroll(object sender, EventArgs e)
        {
            Light.Position.X = tbLightX.Value;
            lLightPosX.Text = tbLightX.Value.ToString();
            glControl.Invalidate();
        }

        private void tbLightY_Scroll(object sender, EventArgs e)
        {
            Light.Position.Y = tbLightY.Value;
            lLightPosY.Text = tbLightY.Value.ToString();
            glControl.Invalidate();
        }

        private void tbLightZ_Scroll(object sender, EventArgs e)
        {
            Light.Position.Z = tbLightZ.Value;
            lLightPosZ.Text = tbLightZ.Value.ToString();
            glControl.Invalidate();
        }

        private void tbAmbientFactor_Scroll(object sender, EventArgs e)
        {
            AmbientStrength = tbAmbientFactor.Value / 1000f;
            glControl.Invalidate();
        }

        private void tbTextureOffset_Scroll(object sender, EventArgs e)
        {
            lbTextureOffset.Text = (tbTextureOffset.Value/10f).ToString();
        }
    }
}