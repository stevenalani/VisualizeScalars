using System;
using System.IO;
using OpenTK;
using OpenTK.Graphics.OpenGL4;
using VisualizeScalars.Logging;

namespace VisualizeScalars.Rendering.ShaderImporter
{
    public class ShaderProgram
    {
        public readonly string FsPath;
        public readonly string GeoPath;
        public readonly string VsPath;
        public int ID = -1;


        public ShaderProgram(string vertexPath, string fragmentPath)
        {
            VsPath = vertexPath;
            FsPath = fragmentPath;
        }

        public ShaderProgram(string vertexPath, string fragmentPath, string geometryPath)
        {
            VsPath = vertexPath;
            FsPath = fragmentPath;
            GeoPath = geometryPath;
        }

        public bool HasErrors { get; set; }
        public bool IsCompiled { get; set; }

        public void SetupShader()
        {
            ID = GL.CreateProgram();
            var vertexShader = CompileVertexShader(VsPath);
            var fragmentShader = CompileFragmentShader(FsPath);
            if (!string.IsNullOrEmpty(GeoPath))
            {
                var geometryShader = CompileGeometryShader(GeoPath);
            }

            LinkShadersToProgram(vertexShader, fragmentShader);
        }

        public void Recompile()
        {
            var vertexShader = CompileVertexShader(VsPath);
            var fragmentShader = CompileFragmentShader(FsPath);
            if (!string.IsNullOrEmpty(GeoPath))
            {
                var geometryShader = CompileGeometryShader(GeoPath);
            }

            LinkShadersToProgram(vertexShader, fragmentShader);
        }

        private object CompileGeometryShader(string geoPath)
        {
            var shader = GL.CreateShader(ShaderType.GeometryShader);
            var shaderString = File.ReadAllText(geoPath).Replace("\n", "\n ");
            GL.ShaderSource(shader, shaderString);
            GL.CompileShader(shader);
            int geoSuccess;
            GL.GetShader(shader, ShaderParameter.CompileStatus, out geoSuccess);
            Console.WriteLine("Geometry Shader compiled: " + (geoSuccess == 1 ? "YES" : "NO"));
            if (geoSuccess == 0)
            {
                string errorLog;
                GL.GetShaderInfoLog(shader, out errorLog);
                Console.WriteLine("Error:\n" + errorLog);
                HasErrors = true;
            }

            return shader;
        }

        private int CompileVertexShader(string vertexPath)
        {
            var vertexShader = GL.CreateShader(ShaderType.VertexShader);
            var shaderString = File.ReadAllText(vertexPath).Replace("\n", "\n ");
            GL.ShaderSource(vertexShader, shaderString);
            GL.CompileShader(vertexShader);
            int vsSuccess;
            GL.GetShader(vertexShader, ShaderParameter.CompileStatus, out vsSuccess);
            Console.WriteLine("Vertex Shader compiled: " + (vsSuccess == 1 ? "YES" : "NO"));
            if (vsSuccess == 0)
            {
                string errorLog;
                GL.GetShaderInfoLog(vertexShader, out errorLog);
                Console.WriteLine("Error:\n" + errorLog);
                HasErrors = true;
            }

            return vertexShader;
        }

        private int CompileFragmentShader(string fragmentPath)
        {
            var fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
            var shaderString = File.ReadAllText(fragmentPath);
            GL.ShaderSource(fragmentShader, shaderString);
            GL.CompileShader(fragmentShader);
            int vsSuccess;
            GL.GetShader(fragmentShader, ShaderParameter.CompileStatus, out vsSuccess);
            Console.WriteLine("Fragment Shader compiled: " + (vsSuccess == 1 ? "YES" : "NO"));
            if (vsSuccess == 0)
            {
                string errorLog;
                GL.GetShaderInfoLog(fragmentShader, out errorLog);
                Console.WriteLine("Error:\n" + errorLog);
                HasErrors = true;
            }

            return fragmentShader;
        }

        private void LinkShadersToProgram(int vertexShader, int fragmentShader)
        {
            GL.AttachShader(ID, vertexShader);
            GL.AttachShader(ID, fragmentShader);
            GL.LinkProgram(ID);
            int linkSucceed;
            GL.GetProgram(ID, GetProgramParameterName.LinkStatus, out linkSucceed);
            if (linkSucceed == 1)
            {
                HasErrors = false;
                IsCompiled = true;
            }
            else
            {
                var errorLog = string.Empty;
                errorLog = GL.GetProgramInfoLog(ID);
                DebugHelpers.Log("Linking Error:", errorLog);
                HasErrors = true;
            }

            GL.DeleteShader(vertexShader);
            GL.DeleteShader(fragmentShader);
        }

        public void SetUniformMatrix4X4(string name, Matrix4 matrix)
        {
            if (!IsCompiled) return;

            var location = GL.GetUniformLocation(ID, name);
            GL.UniformMatrix4(location, false, ref matrix);
        }

        public void SetUniformFloat(string name, float _float)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform1(location, _float);
        }
        public void SetUniformInt(string name, int _int)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform1(location, _int);
        }

        public void SetUniformVector2(string name, Vector2 vec2)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform2(location, vec2);
        }

        public void SetUniformVector3(string name, Vector3 vec3)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform3(location, vec3.X, vec3.Y, vec3.Z);
        }

        public void SetUniformVector4(string name, Vector4 vec4)
        {
            if (!IsCompiled) return;
            var location = GL.GetUniformLocation(ID, name);
            GL.Uniform4(location, vec4);
        }

        public void Use()
        {
            if (!IsCompiled) return;
            GL.UseProgram(ID);
        }

        public static void unuse()
        {
            GL.UseProgram(0);
        }
    }
}