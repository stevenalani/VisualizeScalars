using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Rendering.Models.Voxel;

namespace VisualizeScalars.DataQuery
{
    public class BaseGridCell : IVolumeData
    {
        public readonly Dictionary<string, float> Scalars = new Dictionary<string, float>();
        private bool _isSet;

        public virtual Vector4 ColorMapping => new Vector4(0.5f, 0.5f, 0.5f, 1);
        public bool IsSet => Scalars.Count > 0;

        public float Value(string property)
        {
            return Scalars.ContainsKey(property) ? Scalars[property] : Single.MinValue;
        }

        public void Value(string property, float value)
        {
            if (Scalars.ContainsKey(property))
                Scalars[property] = value;
            else
                Scalars.Add(property, value);
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;
            var cell = (BaseGridCell) obj;
            var cellkeys = cell.Scalars.Keys.Select(x => x).ToArray().All(x => Scalars.ContainsKey(x));
            var cellvalues = cell.Scalars.Values.Select(x => x).ToArray().All(x => Scalars.ContainsValue(x));
            return cellvalues && cellkeys;
        }

        public override int GetHashCode()
        {
            return Tuple.Create(Scalars.Keys, Scalars.Values).GetHashCode();
        }
    }

    public class GridCell : BaseGridCell
    {
        /// <summary>
        ///     Höhen informationen
        /// </summary>
        public double Height
        {
            get => Value("Height");
            set => Value("Height", (float) value);
        }

        public double Temperature
        {
            get => Value("Temperature");
            set => Value("Temperature", (float) (double.IsNaN(value) ? 0.0 : value));
        }

        public double Pressure
        {
            get => Value("Pressure");
            set => Value("Pressure", (float) (double.IsNaN(value) ? 0.0 : value));
        }

        public double Humidity
        {
            get => Value("Humidity");
            set => Value("Humidity", (float) (double.IsNaN(value) ? 0.0 : value));
        }

        public double ParticulateMatter2_5
        {
            get => Value("ParticulateMatter2_5");
            set => Value("ParticulateMatter2_5", (float) (double.IsNaN(value) ? 0.0 : value));
        }

        public double ParticulateMatter10
        {
            get => Value("ParticulateMatter10");
            set => Value("ParticulateMatter10", (float) (double.IsNaN(value) ? 0.0 : value));
        }

        public override Vector4 ColorMapping => new Vector4(0.5f, 0.5f, 0.5f, 1f);
    }
}