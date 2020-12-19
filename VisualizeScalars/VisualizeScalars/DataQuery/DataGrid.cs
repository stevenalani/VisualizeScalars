using System;
using System.Collections.Generic;
using System.Linq;

namespace VisualizeScalars.DataQuery
{
    public class DataGrid<T> where T: BaseGridCell,new()
    {
        
        private Dictionary<string, double> Minimums = new Dictionary<string, double>();
        private Dictionary<string, double> Maximums = new Dictionary<string, double>();
        public int Height => Grid.GetLength(1);
        public int Width => Grid.GetLength(0);
        public double South { get; private set; }
        public double West { get; private set; }
        public double North => South + (GridCellsize * Height);
        public double East => West + (GridCellsize * Width);

        public double GridCellsize { get; set; }
        public string[] PropertyNames => Enumerable.Cast<T>(Grid).SelectMany(x => x.Scalars.Keys).Distinct().ToArray();
        public T[,] Grid { get; private set; }

        public DataGrid(T[,] grid, double cellSize, double latitudeSouth = 0.0, double longitudeWest = 0.0)
        {
            this.Grid = grid;
            var linearSet = grid.Cast<T>();
            foreach (var gridCell in linearSet)
            {
                foreach (var gridCellScalar in gridCell.Scalars)
                {
                    if (Minimums.ContainsKey(gridCellScalar.Key) && Minimums[gridCellScalar.Key] > gridCellScalar.Value)
                        Minimums[gridCellScalar.Key] = double.IsNaN(gridCellScalar.Value)?0.0:gridCellScalar.Value;
                    else if(!Minimums.ContainsKey(gridCellScalar.Key))
                    {
                        Minimums.Add(gridCellScalar.Key,gridCellScalar.Value);
                    }
                    if (Maximums.ContainsKey(gridCellScalar.Key) && Maximums[gridCellScalar.Key] < gridCellScalar.Value)
                        Maximums[gridCellScalar.Key] = double.IsNaN(gridCellScalar.Value) ? 0.0 : gridCellScalar.Value;
                    else if (!Maximums.ContainsKey(gridCellScalar.Key))
                    {
                        Maximums.Add(gridCellScalar.Key, gridCellScalar.Value);
                    }
                }
            }
            
            GridCellsize = cellSize;
            South = latitudeSouth;
            West = longitudeWest;
        }

        public float[,] GetDataGrid(string property, bool normalized = false)
        {
            float[,] grid = new float[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (normalized)
                    {
                        grid[x, y] = (float)((Grid[x, y].Scalars[property] - Minimums[property]) /
                                                   (Maximums[property] - Minimums[property]));
                    }
                    else
                    {
                        grid[x, y] = (float)Grid[x, y].Scalars[property];
                    }
                }
            }
            return grid;
        }

        public T this[int x, int y]
        {
            get => Grid[x, y];
            set => Grid[x, y] = value;
        }

        public double Min(string property)
        {
            return Minimums[property];
        }
        public double Max(string property)
        {
            return Maximums[property];
        }

        public DataGrid<T> Select(string[] strings)
        {
            T[,] newCells = new T[Width,Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newCells[x,y] = new T();
                    foreach (var property in strings)
                    {
                        newCells[x, y].Value(property, Grid[x, y].Value(property));
                    }
                }
            }
            return new DataGrid<T>(newCells,GridCellsize,South,West);
        }
    }

}