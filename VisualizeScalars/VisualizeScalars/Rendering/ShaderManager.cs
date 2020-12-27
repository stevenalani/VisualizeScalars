using System;
using System.Collections.Generic;
using System.Linq;
using VisualizeScalars.Rendering.Models;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering
{
    public class ShaderManager
    {
        private readonly Dictionary<ShaderProgram, List<Type>> _shaders = new Dictionary<ShaderProgram, List<Type>>();
        private bool _hasShaderUpdates;

        public void InitPrograms()
        {
            if (!_hasShaderUpdates)
                return;
            var shaderProgs = _shaders.Keys.Where(X => !X.IsCompiled);
            foreach (var shaderProgram in shaderProgs) shaderProgram.SetupShader();
            _hasShaderUpdates = false;
        }

        public void AddShader(Type[] modeltype, string vsShaderPath, string fsShaderPath = "")
        {
            var shader = ShaderImporter.ShaderImporter.LoadShader(vsShaderPath, fsShaderPath);
            _shaders.Add(shader, new List<Type>(modeltype));
            _hasShaderUpdates = true;
        }

        public void AddShader(Type modeltype, string vsShaderPath, string fsShaderPath = "")
        {
            var shader = ShaderImporter.ShaderImporter.LoadShader(vsShaderPath, fsShaderPath);
            _shaders.Add(shader, new List<Type> {modeltype});
            _hasShaderUpdates = true;
        }

        public void AddShader(Type modeltype, string vsShaderPath, string fsShaderPath, string geomShaderPath)
        {
            var shader = ShaderImporter.ShaderImporter.LoadShader(vsShaderPath, fsShaderPath, geomShaderPath);
            _shaders.Add(shader, new List<Type> {modeltype});
            _hasShaderUpdates = true;
        }

        public ShaderProgram GetForType(Type type)
        {
            return _shaders.First(x => x.Value.Contains(type)).Key;
        }

        public ShaderProgram GetFirst()
        {
            return _shaders.Keys.First();
        }

        public ShaderProgram GetShader(Type type)
        {
            return _shaders.First(x => x.Value.Contains(type)).Key;
        }

        public void AssignTypeToShader(Type type, string vertexShaderPath)
        {
            var shadertuple = _shaders.FirstOrDefault(x => x.Key.VsPath == vertexShaderPath);
            if (shadertuple.Key == null || shadertuple.Value == null)
                return;
            shadertuple.Value.Add(type);
        }

        public ShaderProgram GetShaderForModel(Model model)
        {
            return _shaders.FirstOrDefault(x => x.Value.Contains(model.GetType())).Key;
        }
    }
}