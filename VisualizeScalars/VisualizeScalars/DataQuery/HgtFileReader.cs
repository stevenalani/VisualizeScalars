using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VisualizeScalars.DataQuery
{
    /// <summary>
    ///     Sampling- Rate des SRTM- Sensors
    /// </summary>
    public enum SamplingRate
    {
        Onearcsecond = 1,
        Threearcseconds = 3,
        Thirtyarcseconds = 30
    }

    public class HgtFileReader
    {
        public static readonly int NODATA = -32768;

        private readonly Dictionary<SamplingRate, int> _samplesLookup = new Dictionary<SamplingRate, int>
        {
            {SamplingRate.Onearcsecond, 3601},
            {SamplingRate.Threearcseconds, 1201},
            {SamplingRate.Thirtyarcseconds, 121}
        };

        private readonly SamplingRate _samplingRate;
        private readonly string _workingDirectory;
        private BinaryReader _binaryReader;
        private FileStream _fileStream;


        /// <summary>
        ///     Erstellt eine Instanz des HgtFileReader zum Lesen von .hgt- Dateien
        /// </summary>
        /// <param name="workingDirectory">Ordner mit .hgt- Dateien</param>
        /// <param name="samplingRate">Sampling Rate des Sensors</param>
        public HgtFileReader(string workingDirectory, SamplingRate samplingRate = SamplingRate.Onearcsecond)
        {
            _samplingRate = samplingRate;
            _workingDirectory = workingDirectory;
        }

        public int Samples => _samplesLookup[_samplingRate];
        private string CurrentFile => _fileStream?.Name;

        public short[,] GetAllElevationData(string path)
        {
            var result = new short[(uint) Samples, (uint) Samples];
            if (string.IsNullOrEmpty(path))
                return result;
            _binaryReader?.Close();
            if (CurrentFile != path) _fileStream?.Close();
            _fileStream = File.Open(path, FileMode.Open);
            _binaryReader = new BinaryReader(_fileStream);
            for (var y = 0; y < (uint) Samples; y++)
            for (var x = 0; x < (uint) Samples; x++)
            {
                var buffer = _binaryReader.ReadBytes(2);
                var value = (buffer[0] << 8) | buffer[1];
                result[x, y] = (short) value;
            }

            _binaryReader.Close();
            _fileStream.Close();
            _binaryReader.Dispose();
            _fileStream.Dispose();
            return result;
        }

        public int GetElevationAtCoordinate(double latitude, double longitude)
        {
            var hgtDirectories = Directory.EnumerateDirectories(_workingDirectory);
            var hgtName = getHgtTileName(latitude, longitude);
            var hgtDirectory = hgtDirectories.FirstOrDefault(x => x.Contains(hgtName));
            if (string.IsNullOrEmpty(hgtDirectory)) return NODATA;
            var hgtFile = Directory.GetFiles(hgtDirectory, hgtName + ".hgt").FirstOrDefault();
            if (string.IsNullOrEmpty(hgtFile)) return NODATA;
            UpdateReader(hgtFile);
            var row = LatToRow(latitude);
            var col = LngToCol(longitude);
            var height = ReadAt(row, col);
            return height;
        }

        public int[,] GetElevationForArea(double latitudeSouth, double longitudeWest, double latitudeNorth,
            double longitudeEast, double cellSize)
        {
            var hgtDirectories = Directory.EnumerateDirectories(_workingDirectory);

            var tileCountX = (int) longitudeEast - (int) longitudeWest + 1;

            var tileCountY = (int) latitudeNorth - (int) latitudeSouth + 1;

            var cellCountXSum = (int) (tileCountX / cellSize);
            var cellCountYSum = (int) (tileCountY / cellSize);
            var heights = new int[cellCountXSum, cellCountYSum];
            var cellsY = (int) Math.Ceiling(Math.Abs(latitudeNorth - latitudeSouth) / cellSize);
            var cellsX = (int) Math.Ceiling(Math.Abs(longitudeEast - longitudeWest) / cellSize);
            var results = new int[cellsX, cellsY];
            for (var tileIdxY = 0; tileIdxY < tileCountY; tileIdxY++)
            for (var tileIdxX = 0; tileIdxX < tileCountX; tileIdxX++)
            {
                var tileName = getHgtTileName(latitudeSouth + tileIdxY, longitudeWest + tileIdxX);
                var hgtDirectory = hgtDirectories.FirstOrDefault(dir => dir.Contains(tileName));
                var hgtFile = Directory.GetFiles(hgtDirectory, tileName + ".hgt").FirstOrDefault();
                UpdateReader(hgtFile);
                var data = GetAllElevationData(hgtFile);
                for (var i = 0; i < Samples; i++)
                for (var j = 0; j < Samples; j++)
                    heights[i + tileIdxX * Samples, j + tileIdxY * Samples] = data[i, j];
            }

            var startRow = LatToRow(latitudeNorth);
            var startCol = LngToCol(longitudeWest);
            for (var i = 0; i < cellsY; i++)
            for (var j = 0; j < cellsX; j++)
                results[j, i] = heights[j + startCol, i + startRow];
            return results;
        }

        public void UpdateReader(string hgtFile)
        {
            if (_fileStream == null || !_fileStream.CanRead || CurrentFile != hgtFile)
            {
                Close();
                _fileStream = File.OpenRead(hgtFile);
                _binaryReader = new BinaryReader(_fileStream);
            }
        }

        public void Open(string hgtFile)
        {
            UpdateReader(hgtFile);
        }

        public int ReadAt(int row, int col)
        {
            long bytepos = (Samples * row + col) * 2;
            _fileStream.Position = bytepos;
            return ReadHeight();
        }

        private int ReadHeight()
        {
            var buffer = _binaryReader.ReadBytes(2);
            var value = (buffer[0] << 8) | buffer[1];
            return value;
        }

        public int LatToRow(double lat)
        {
            var frac = lat - (int) lat;
            return Samples - 1 - (int) (frac / (1.0 / Samples));
        }

        public int LngToCol(double lng)
        {
            var frac = lng - (int) lng;
            return (int) (frac / (1.0 / Samples));
        }

        public static string getHgtTileName(double latitude, double longitude)
        {
            var latNamePart = latitude < 0 ? $"S{(int) Math.Abs(latitude):00}" : $"N{(int) Math.Abs(latitude):00}";
            var lngNamePart = longitude < 0 ? $"W{(int) Math.Abs(longitude):000}" : $"E{(int) Math.Abs(longitude):000}";
            return latNamePart + lngNamePart;
        }

        public void Close()
        {
            _binaryReader?.Close();
            _fileStream?.Close();
        }
    }
}