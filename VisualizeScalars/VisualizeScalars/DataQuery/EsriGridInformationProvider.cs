using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace VisualizeScalars.DataQuery
{
    public class EsriGridInformationProvider
    {
        public static readonly int HeaderRowCount = 6;
        private readonly Dictionary<string, string> _headers = new Dictionary<string, string>();
        private readonly string fileName;

        /// <summary>
        ///     Diese Klasse liest die HeaderInformationen aus einer IGBP .dat Datei
        /// </summary>
        /// <param name="file">Eine Datei aus dem Set</param>
        public EsriGridInformationProvider(string file)
        {
            fileName = file;
            var fsRead = new FileStream(file, FileMode.Open, FileAccess.Read);
            var streamReader = new StreamReader(fsRead);
            var lineNo = 1;
            while (lineNo <= HeaderRowCount)
            {
                var line = streamReader.ReadLine();
                var headerParts = line.Split(' ');
                _headers.Add(headerParts[0], headerParts[1]);

                lineNo++;
            }

            fsRead.Close();
            streamReader.Close();
            ConvertDataFile();
        }

        public int Columns => _headers.ContainsKey("ncols") ? Convert.ToInt32(_headers["ncols"]) : -1;
        public int Rows => _headers.ContainsKey("nrows") ? Convert.ToInt32(_headers["nrows"]) : -1;

        public double CellSize => _headers.ContainsKey("cellsize")
            ? Convert.ToDouble(_headers["cellsize"], CultureInfo.InvariantCulture)
            : double.NaN;

        public double LastCellX => _headers.ContainsKey("xllcorner")
            ? Convert.ToDouble(_headers["xllcorner"], CultureInfo.InvariantCulture)
            : double.NaN;

        public double LastCellY => _headers.ContainsKey("yllcorner")
            ? Convert.ToDouble(_headers["yllcorner"], CultureInfo.InvariantCulture)
            : double.NaN;

        public double FirstCellX => LastCellX;
        public double FirstCellY => Math.Ceiling(LastCellY + Rows * CellSize);

        public double NoDataValue => _headers.ContainsKey("NODATA_value")
            ? Convert.ToDouble(_headers["NODATA_value"], CultureInfo.InvariantCulture)
            : double.NaN;

        public string ConvertedDataFile => fileName.Replace(Path.GetExtension(fileName), "");

        internal void ConvertDataFile()
        {
            if (!File.Exists(ConvertedDataFile))
            {
                var streamReader = new StreamReader(File.OpenRead(fileName));
                var fsWrite = new FileStream(ConvertedDataFile, FileMode.CreateNew, FileAccess.Write);
                var writer = new BinaryWriter(fsWrite);
                string dataline;
                var lineCnt = 1;
                while ((dataline = streamReader.ReadLine()) != null)
                {
                    if (lineCnt <= HeaderRowCount)
                    {
                        lineCnt++;
                        continue;
                    }

                    var values = dataline.Split(' ');
                    foreach (var value in values) writer.Write(Convert.ToSingle(value, CultureInfo.InvariantCulture));
                }

                fsWrite.Close();
                writer.Close();
            }
        }

        public int LatToRow(double latitude)
        {
            if (latitude < LastCellY || latitude > FirstCellY) return -1;

            return (int) Math.Abs(Math.Round((FirstCellY - latitude) / CellSize));
        }

        public int LngToCol(double longitude)
        {
            if (longitude < LastCellX || longitude > Math.Abs(FirstCellX)) return -1;
            return (int) Math.Abs(Math.Round((FirstCellX - longitude) / CellSize));
        }

        public double InformationForCoordinate(double latitude, double longitude)
        {
            var row = LatToRow(latitude); // row excel string = row+1!
            var col = LngToCol(longitude); // col excel string = col+1!
            return ReadAt(row, col);
        }

        public double[,] InformationForArea(double latitudeSouth, double longitudeWest, double latitudeNorth,
            double longitudeEast, double cellSize)
        {
            int indexX = 0, indexY = 0;
            var cellCountX = (int) ((longitudeEast - longitudeWest) / cellSize) + 1;
            var cellCountY = (int) ((latitudeNorth - latitudeSouth) / cellSize) + 1;
            var result = new double[cellCountX, cellCountY];
            var fs = new FileStream(ConvertedDataFile, FileMode.Open, FileAccess.Read);
            var reader = new BinaryReader(fs);
            for (var latitude = latitudeSouth; latitude < latitudeNorth; latitude += cellSize)
            {
                for (var longitude = longitudeWest; longitude < longitudeEast; longitude += cellSize)
                {
                    var row = LatToRow(latitude); // row excel string = row+1!
                    var col = LngToCol(longitude); // col excel string = col+1!
                    long position = (row * Columns + col) * sizeof(float);
                    fs.Position = position;
                    result[indexX, indexY] = Convert.ToDouble(reader.ReadSingle());
                    indexX++;
                }

                indexX = 0;
                indexY++;
            }

            reader.Close();
            fs.Close();
            return result;
        }

        public double ReadAt(int row, int col)
        {
            long position = (row * Columns + col) * sizeof(float);
            var fs = new FileStream(ConvertedDataFile, FileMode.Open, FileAccess.Read) {Position = position};
            var reader = new BinaryReader(fs);
            var value = reader.ReadSingle();
            reader.Close();
            fs.Close();
            return Convert.ToDouble(value);
        }
    }
}