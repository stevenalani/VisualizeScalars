using System;
using System.Collections.Generic;
using System.Linq;
using OpenTK;
using VisualizeScalars.Rendering.Models.Voxel;

namespace VisualizeScalars.DataQuery
{
    public class BaseGridCell : IVolumeData
    {
        public Dictionary<string, float> Scalars = new Dictionary<string, float>();
        public float Value(string property) => Scalars.ContainsKey(property)? Scalars[property]:0.0f;

        public void Value(string property, float value)
        {
            if (Scalars.ContainsKey(property))
            {
                Scalars[property] = value;
            }
            else
            {
                Scalars.Add(property,value);
            }
            
        }

        public virtual Vector4 ColorMapping => new Vector4(0.5f, 0.5f, 0.5f,1);

        public virtual bool IsSetVoxel()
        {
            return Scalars.Count > 0 && Scalars.First().Value > 0;
        }

    }

    public class GridCell : BaseGridCell
    {
        /// <summary>
        /// Höhen informationen
        /// </summary>
        public double Height
        {
            get => Value("Height");
            set => Value("Height", (float)value);
        }
        /// <summary>
        /// Schüttdichte - wie Kompakt ist ein Boden
        /// </summary>
        public double BulkDensity
        {
            get => Value("BulkDensity");
            set => Value("BulkDensity", (float)value);
        }
        /// <summary>
        /// Wassermänge die der durchtränkte Boden halten kann (2-3 Tage)
        /// </summary>
        public double FieldCapacity
        {
            get => Value("FieldCapacity");
            set => Value("FieldCapacity", (float)(double.IsNaN(value)?0.0:value));
        }
        /// <summary>
        /// Wassermänge welche für Pflanzen zur Verfügung steht
        /// </summary>
        public double ProfileAvailableWaterCapacity
        {
            get => Value("ProfileAvailableWaterCapacity");
            set => Value("ProfileAvailableWaterCapacity", (float)value);
        }
        /// <summary>
        /// Bodenkohlenstoff Dichte - bestimmt die Fruchtbarkeit maßgeblich
        /// </summary>
        public double SoilCarbonDensity
        {
            get => Value("SoilCarbonDensity");
            set => Value("SoilCarbonDensity", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        /// <summary>
        /// Wärmespeicherzahl 
        /// </summary>
        public double ThermalCapacity
        {
            get => Value("ThermalCapacity");
            set => Value("ThermalCapacity", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        /// <summary>
        /// Stickstoffgehalt
        /// </summary>
        public double TotalNitrogenDensity
        {
            get => Value("TotalNitrogenDensity");
            set => Value("TotalNitrogenDensity", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        /// <summary>
        /// Welkepunkt
        /// </summary>
        public double WiltingPoint
        {
            get => Value("WiltingPoint");
            set => Value("WiltingPoint", (float)(double.IsNaN(value) ? 0.0 : value));
        }

        public double Temperature
        {
            get => Value("Temperature");
            set => Value("Temperature", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        public double Pressure
        {
            get => Value("Pressure");
            set => Value("Pressure", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        public double Humidity
        {
            get => Value("Humidity");
            set => Value("Humidity", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        public double ParticulateMatter2_5
        {
            get => Value("ParticulateMatter2_5");
            set => Value("ParticulateMatter2_5", (float)(double.IsNaN(value) ? 0.0 : value));
        }
        public double ParticulateMatter10
        {
            get => Value("ParticulateMatter10");
            set => Value("ParticulateMatter10", (float)(double.IsNaN(value) ? 0.0 : value));
        }

        public override Vector4 ColorMapping => new Vector4(0.5f, 0.5f, 0.5f, 1f);

        public override bool IsSetVoxel()
        {
            return true;
        }
    }

    
}