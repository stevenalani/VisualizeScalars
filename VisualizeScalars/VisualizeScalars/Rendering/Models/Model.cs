using System;
using OpenTK;
using VisualizeScalars.Helpers;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering.Models
{
    public abstract class Model : IDisposable
    {
        private static int _nextId;
        public readonly int ID;


        public Vector3 Direction = Vector3.UnitZ;
        public string name;

        public Vector3 PivotPoint = Vector3.Zero;

        public Vector3 Position = Vector3.Zero;
        protected internal Vector3 Rotations = Vector3.Zero;
        public Vector3 Scales = new Vector3(1f,1f,1f);
        public Mesh Mesh { get; set; }
        protected Model(string name)
        { 
            ID = _nextId++;
            this.name = name;
        }
        protected Model()
        {
            
            ID = _nextId++;
            name = ID.ToString();
        }

        public Matrix4 Modelmatrix => Matrix4.Identity *
                                      Matrix4.CreateTranslation(-PivotPoint)*
                                      MathHelpers.GetRotation(Rotations.X, Rotations.Y, Rotations.Z) *
                                      Matrix4.CreateScale(Scales) *
                                      Matrix4.CreateTranslation(Position);

        public bool IsReady { get; set; } = false;

        public virtual void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public abstract void Draw(ShaderProgram shaderProgram,Action<ShaderProgram> setUniforms = null);

        public abstract void InitBuffers();


        public void MoveForward(float distance)
        {
            Position += Direction * distance;
        }

        public void rotateX(float yaw)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yaw, 0f, 0f);
            Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.X += yaw;
        }

        public void rotateY(float pitch)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(0f, pitch, 0f);
            Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Y += pitch;
        }

        public void rotateZ(float roll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(0f, 0f, roll);
            Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
            Rotations.Z += roll;
        }

        public void rotate(float yaw, float pitch, float roll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yaw, pitch, roll);
            Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }

        public void rotate(Vector3 yawpitchroll)
        {
            var rotationMatrix4 = MathHelpers.GetRotation(yawpitchroll.X, yawpitchroll.Y, yawpitchroll.Z);
            Direction = Vector3.TransformNormal(Direction, rotationMatrix4);
        }
    }
}