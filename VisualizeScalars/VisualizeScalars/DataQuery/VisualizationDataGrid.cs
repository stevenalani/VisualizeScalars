namespace VisualizeScalars.DataQuery
{
    public class VisualizationDataGrid
    {
        #region staticMinMax
        public static double MinHeight;
        public static double MaxHeight;
        public static double MinBulkDensity = 0.0;
        public static double MaxBulkDensity = 2.6;
        public static double MinWiltingPoint = 0;
        public static double MaxWiltingPoint = 286.28;
        public static double MinFieldCapacity = 0.0;
        public static double MaxFieldCapacity = 813.07;
        public static double MinProfileAvailableWaterCapacity = 0.0;
        public static double MaxProfileAvailableWaterCapacity = 527.12;
        public static double MinSoilCarbonDensity = 0.0;
        public static double MaxSoilCarbonDensity = 82.1512;
        public static double MinThermalCapacity = 0.0;
        public static double MaxThermalCapacity = 2.02;
        public static double MinTotalNitrogenDensity = .0;
        public static double MaxTotalNitrogenDensity = 6913.14;
        #endregion

        public int Height => Grid.GetLength(1);
        public int Width => Grid.GetLength(0);
        public double South { get; private set; }
        public double West { get; private set; }
        public double North => South + (gridCellsize * Height);
        public double East => West + (gridCellsize * Width);

        public double gridCellsize { get; private set; }
        public ScalarSet[,] Grid { get; set; }

        public VisualizationDataGrid(ScalarSet[,] grid, double cellSize, double latitudeSouth, double longitudeWest)
        {
            this.Grid = grid;
            gridCellsize = cellSize;
            South = latitudeSouth;
            West = longitudeWest;
        }
        public float[,] HeightGrid(bool normalized = false)
        {
            float[,] heightGrid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        heightGrid[x, y] = (float)((Grid[x, y].Height - MinHeight) /
                                                   (MaxHeight - MinHeight));
                    }
                    else
                    {
                        heightGrid[x, y] = (float)Grid[x, y].Height;
                    }
                }
            }
            return heightGrid;
        }
        public float[,] BulkDensityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].BulkDensity - MinBulkDensity) /
                                             (MaxBulkDensity - MinBulkDensity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].BulkDensity;
                    }
                }
            }
            return grid;
        }
        public float[,] FieldCapacityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].FieldCapacity - MinFieldCapacity) /
                                             (MaxFieldCapacity - MinFieldCapacity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].FieldCapacity;
                    }
                }
            }
            return grid;
        }
        public float[,] ProfileAvailableWaterCapacityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].ProfileAvailableWaterCapacity - MinProfileAvailableWaterCapacity) /
                                             (MaxProfileAvailableWaterCapacity - MinProfileAvailableWaterCapacity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].ProfileAvailableWaterCapacity;
                    }
                }
            }

            return grid;
        }
        public float[,] WiltingPointGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].WiltingPoint - MinWiltingPoint) /
                                             (MaxWiltingPoint - MinWiltingPoint));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].WiltingPoint;
                    }
                }
            }
            
            return grid;
        }
        public float[,] SoilCarbonDensityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].SoilCarbonDensity - MinSoilCarbonDensity) /
                                             (MaxSoilCarbonDensity - MinSoilCarbonDensity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].SoilCarbonDensity;
                    }
                }
            }

            return grid;
        }
        public float[,] ThermalCapacityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].ThermalCapacity - MinThermalCapacity) /
                                             (MaxThermalCapacity - MinThermalCapacity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].ThermalCapacity;
                    }
                }
            }
            
            return grid;
        }
        public float[,] TotalNitrogenDensityGrid(bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].TotalNitrogenDensity - MinTotalNitrogenDensity) /
                                             (MaxTotalNitrogenDensity - MinTotalNitrogenDensity));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].TotalNitrogenDensity;
                    }
                }
            }
            
            return grid;
        }

        public ScalarSet this[int x, int y]
        {
            get { return Grid[x, y]; }
            set { Grid[x, y] = value; }
        }
    }
}