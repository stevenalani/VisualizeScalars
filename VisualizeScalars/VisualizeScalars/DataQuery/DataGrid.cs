using System;
using System.Collections.Generic;
using System.Linq;
using VisualizeScalars.Helpers;

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
        public DataGrid<K> Select<K>(string[] strings) where K : BaseGridCell, new()
        {
            K[,] newCells = new K[Width, Height];
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    newCells[x, y] = new K();
                    foreach (var property in strings)
                    {
                        newCells[x, y].Value(property, Grid[x, y].Value(property));
                    }
                }
            }
            return new DataGrid<K>(newCells, GridCellsize, South, West);
        }
        public DataGrid<T> Scaled(float scale, bool normalize = false,
            bool trim = false)
        {

            int oldWidth = Grid.GetLength(0);
            int oldHeight = Grid.GetLength(1);
            int newWidth = (int)(oldWidth * scale);
            int newHeight = (int)(oldHeight * scale);

            T[,] newData = new T[newWidth, newHeight];
            Dictionary<string, float[,]> scalarGrids = new Dictionary<string, float[,]>();
            foreach (var scalarsKey in Grid[0, 0].Scalars.Keys)
            {
                scalarGrids.Add(scalarsKey, GetDataGrid(scalarsKey, normalize).Scale(scale));
            }
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    var cell = new T();
                    foreach (var scalarGrid in scalarGrids)
                    {
                        var val = scalarGrid.Value[x, y];
                        cell.Value(scalarGrid.Key, trim ? (float)Math.Floor(val) : val);
                    }

                    newData[x, y] = cell;
                }
            }

            return new DataGrid<T>(newData, GridCellsize / scale, South, West);
        }  
        public void Scale(float scale, bool normalize = false,
            bool trim = false)
        {

            int oldWidth = Grid.GetLength(0);
            int oldHeight = Grid.GetLength(1);
            int newWidth = (int)(oldWidth * scale);
            int newHeight = (int)(oldHeight * scale);

            T[,] newData = new T[newWidth, newHeight];
            Dictionary<string, float[,]> scalarGrids = new Dictionary<string, float[,]>();
            foreach (var scalarsKey in Grid[0, 0].Scalars.Keys)
            {
                scalarGrids.Add(scalarsKey, GetDataGrid(scalarsKey, normalize).Scale(scale));
            }
           
            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    var cell = new T();
                    foreach (var scalarGrid in scalarGrids)
                    {
                        var val = scalarGrid.Value[x, y];
                        cell.Value(scalarGrid.Key, trim ? (float)Math.Floor(val) : val);
                    }

                    newData[x, y] = cell;
                }
            }

            GridCellsize /= scale;
            Grid = newData;
        }
        public void RemoveSet(string? property)
        {
            if(property == null)
                return;
            Minimums.Remove(property);
            Maximums.Remove(property);
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Grid[x, y].Scalars.Remove(property);
                }
            }
        }

        public float ValueNormalized(in int i, in int i1, string propertyName)
        {
            return (float)((Grid[i, i1].Scalars[propertyName] - Minimums[propertyName]) /
                                 (Maximums[propertyName] - Minimums[propertyName]));
        }
    }

}