namespace VisualizeScalars.DataQuery
{
    public class ScalarSet
    {
        /// <summary>
        /// Latitude der Zelle
        /// </summary>
       public double Latitude { get; set; }
       public double Longitude { get; set; }
        /// <summary>
        /// Höhen informationen
        /// </summary>
        public double Height { get; set; }
        /// <summary>
        /// Schüttdichte - wie Kompakt ist ein Boden
        /// </summary>
        public double BulkDensity { get; set; }
        /// <summary>
        /// Wassermänge die der durchtränkte Boden halten kann (2-3 Tage)
        /// </summary>
        public double FieldCapacity { get; set; }
        /// <summary>
        /// Wassermänge welche für Pflanzen zur Verfügung steht
        /// </summary>
        public double ProfileAvailableWaterCapacity { get; set; }
        /// <summary>
        /// Bodenkohlenstoff Dichte - bestimmt die Fruchtbarkeit maßgeblich
        /// </summary>
        public double SoilCarbonDensity { get; set; }
        /// <summary>
        /// Wärmespeicherzahl 
        /// </summary>
        public double ThermalCapacity { get; set; }
        /// <summary>
        /// Stickstoffgehalt
        /// </summary>
        public double TotalNitrogenDensity { get; set; }
        /// <summary>
        /// Welkepunkt
        /// </summary>
        public double WiltingPoint { get; set; }

        public double Temperature { get; set; }
        public double Pressure { get; set; }
        public double Humiditiy { get; set; }
        public double ParticulateMatter2_5 { get; set; }
        public double ParticulateMatter10 { get; set; }
    }

    
}