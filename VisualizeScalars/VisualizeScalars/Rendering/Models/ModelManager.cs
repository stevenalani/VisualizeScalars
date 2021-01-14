using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Rendering.ShaderImporter;

namespace VisualizeScalars.Rendering.Models
{
    internal class ModelManager
    {
        private readonly Dictionary<int, Model> _models = new Dictionary<int, Model>();
        private readonly ConcurrentQueue<Model> UninitializedModels = new ConcurrentQueue<Model>();


        public bool HasModelUpdates { get; set; }

        public List<Model> GetModelsOrdered(Vector3 position, bool joined = true)
        {
            var models = GetModels(joined);
            return models.OrderByDescending(m => (m.Position - position).Length).ToList();
        }
        public List<Model> GetModels(bool joined = true)
        {
            if (!joined)
                return _models.Values.ToList();
            var res = new List<Model>();
            res.AddRange(UninitializedModels);
            res.AddRange(_models.Values);
            return res.ToList();
        }

        public void AddModel(Model model)
        {
            HasModelUpdates = true;
            UninitializedModels.Enqueue(model);
        }

        public void InitModels()
        {
            if (!UninitializedModels.IsEmpty)
            {
                Model model = null;
                while (!UninitializedModels.IsEmpty)
                {
                    UninitializedModels.TryDequeue(out model);
                    model?.InitBuffers();
                    _models.Add(model.ID, model);
                }
            }

            HasModelUpdates = false;
        }

        public List<Model> GetModel(string name)
        {
            var l1 = _models.Values.Where(x => x.name == name).ToList();
            var l2 = UninitializedModels.Where(x => x.name == name).ToList();
            l1.AddRange(l2);
            return l1;
        }

        public void ClearModels()
        {
            UninitializedModels.Clear();
            _models.Clear();
        }
    }
}