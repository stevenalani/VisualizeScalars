using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VisualizeScalars.DataQuery
{
    public class DataFileQuerent
    {
        private readonly string _bdFile = "bulkdens.dat";

        private readonly string _fcFile = "fieldcap.dat";

        private readonly string _pawcFile = "pawc.dat";

        private readonly string _scFile = "soilcarb.dat";

        private readonly string _tcFile = "thermcap.dat";

        private readonly string _tnFile = "totaln.dat";

        private readonly string _wpFile = "wiltpont.dat";
        public readonly string WorkingDirectory;
        private readonly Dictionary<string, EsriGridInformationProvider> _igbpInformationProviders;

        private readonly AirdataReader airdataReader;
        private readonly EarthdataDownloader earthDataLoader;

        private readonly HgtFileReader hgtFileReader;

        private readonly DirectoryInfo workingDirectoryInfo;


        public DataFileQuerent(string workingDirectory, EarthdataDownloader downloader = null,
            HgtFileReader hgtFileReader = null)
        {
            if (string.IsNullOrEmpty(workingDirectory))
                throw new Exception("Es wurde kein Arbeitsverzeichnis angegeben");

            if (downloader != null)
                workingDirectory = downloader.WorkingDirectory;
            else
                downloader = new EarthdataDownloader(workingDirectory);
            WorkingDirectory = workingDirectory;
            workingDirectoryInfo = new DirectoryInfo(workingDirectory);
            if (hgtFileReader == null) hgtFileReader = new HgtFileReader(WorkingDirectory);
            airdataReader = new AirdataReader(WorkingDirectory);
            EnsureGeneralFilesExist();

            earthDataLoader = downloader;
            this.hgtFileReader = hgtFileReader;
            _igbpInformationProviders = new Dictionary<string, EsriGridInformationProvider>
            {
                {_pawcFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _pawcFile))},
                {_bdFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _bdFile))},
                {_fcFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _fcFile))},
                {_scFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _scFile))},
                {_tcFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _tcFile))},
                {_tnFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _tnFile))},
                {_wpFile, new EsriGridInformationProvider(Path.Combine(WorkingDirectory, _wpFile))}
            };
        }

        private void EnsureGeneralFilesExist()
        {
            var exists = File.Exists(Path.Combine(WorkingDirectory, _pawcFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _bdFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _fcFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _scFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _tcFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _tnFile)) &&
                         File.Exists(Path.Combine(WorkingDirectory, _wpFile));

            if (exists == false) earthDataLoader.DownloadDataSet("soils");
        }

        public void PrepareArea(double latSouth, double lngWest, double latNorth, double lngEast)
        {
            if (latSouth > latNorth)
            {
                var temp = latNorth;
                latNorth = latSouth;
                latSouth = temp;
            }

            if (lngWest > lngEast)
            {
                var temp = lngEast;
                lngEast = lngWest;
                lngWest = temp;
            }

            EnsureAreaExists(latSouth, lngWest, latNorth, lngEast);
            EnsureGeneralFilesExist();
        }

        private void EnsureAreaExists(double latSouth, double lngWest, double latNorth, double lngEast)
        {
            for (var lat = (int) latSouth; lat <= (int) latNorth; lat++)
            for (var lng = (int) lngWest; lng <= (int) lngEast; lng++)
            {
                var hgtTileName = HgtFileReader.getHgtTileName(lat, lng);
                if (!workingDirectoryInfo.GetDirectories().Any(x => x.Name.Contains(hgtTileName)))
                    earthDataLoader.DownloadDataSets(lat + 0.05, lng + 0.05);
            }
        }

        public GridCell GetSoilInformationForCoordinate(double latitude, double longitude)
        {
            var hgtTileName = HgtFileReader.getHgtTileName(latitude, longitude);
            if (!workingDirectoryInfo.GetDirectories().Any(x => x.Name.Contains(hgtTileName)))
                earthDataLoader.DownloadDataSets(latitude, longitude);

            var si = new GridCell
            {
                Height = hgtFileReader.GetElevationAtCoordinate(latitude, longitude),
                ProfileAvailableWaterCapacity =
                    _igbpInformationProviders[_pawcFile].InformationForCoordinate(latitude, longitude),
                BulkDensity = _igbpInformationProviders[_bdFile].InformationForCoordinate(latitude, longitude),
                SoilCarbonDensity = _igbpInformationProviders[_scFile].InformationForCoordinate(latitude, longitude),
                FieldCapacity = _igbpInformationProviders[_fcFile].InformationForCoordinate(latitude, longitude),
                ThermalCapacity = _igbpInformationProviders[_tcFile].InformationForCoordinate(latitude, longitude),
                TotalNitrogenDensity = _igbpInformationProviders[_tnFile].InformationForCoordinate(latitude, longitude),
                WiltingPoint = _igbpInformationProviders[_wpFile].InformationForCoordinate(latitude, longitude)
            };
            return si;
        }

        public double GetGridUnitStep()
        {
            return new[]
            {
                1.0 / hgtFileReader.Samples,
                _igbpInformationProviders[_pawcFile].CellSize,
                _igbpInformationProviders[_bdFile].CellSize,
                _igbpInformationProviders[_scFile].CellSize,
                _igbpInformationProviders[_fcFile].CellSize,
                _igbpInformationProviders[_tcFile].CellSize,
                _igbpInformationProviders[_tnFile].CellSize,
                _igbpInformationProviders[_wpFile].CellSize
            }.Min();
        }

        public AirInformation[,] GetAirDataGrid(double latitudeSouth, double longitudeWest, double latitudeNorth,
            double longitudeEast, double gridUnit)
        {
            //airdataReader.DownloadData();
            return airdataReader.BuildGrid(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
        }

        public GridCell[,] GetDataForArea(double latitudeSouth, double longitudeWest, double latitudeNorth,
            double longitudeEast)
        {
            if (latitudeSouth > latitudeNorth)
            {
                var temp = latitudeNorth;
                latitudeNorth = latitudeSouth;
                latitudeSouth = temp;
            }

            if (longitudeWest > longitudeEast)
            {
                var temp = longitudeEast;
                longitudeEast = longitudeWest;
                longitudeWest = temp;
            }

            var gridUnit = GetGridUnitStep();
            PrepareArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast);

            var cellCountX = (int) ((longitudeEast - longitudeWest) / gridUnit);
            var cellCountY = (int) ((latitudeNorth - latitudeSouth) / gridUnit);
            var airdata = GetAirDataGrid(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            var Height =
                hgtFileReader.GetElevationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] ProfileAvailableWaterCapacity = _igbpInformationProviders[_pawcFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] BulkDensity = _igbpInformationProviders[_bdFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] SoilCarbonDensity = _igbpInformationProviders[_scFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] FieldCapacity = _igbpInformationProviders[_fcFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] ThermalCapacityData = _igbpInformationProviders[_tcFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] TotalNitrogenDensity = _igbpInformationProviders[_tnFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            double[,] WiltingPoint = _igbpInformationProviders[_wpFile]
                .InformationForArea(latitudeSouth, longitudeWest, latitudeNorth, longitudeEast, gridUnit);
            var results = new GridCell[cellCountX, cellCountY];

            for (var j = 0; j < cellCountY; j++)
            for (var i = 0; i < cellCountX; i++)
                results[i, j] = new GridCell
                {
                    Height = Height[i, j],
                    ProfileAvailableWaterCapacity = ProfileAvailableWaterCapacity[i, j],
                    BulkDensity = BulkDensity[i, j],
                    SoilCarbonDensity = SoilCarbonDensity[i, j],
                    FieldCapacity = FieldCapacity[i, j],
                    ThermalCapacity =  ThermalCapacityData[i, j],
                    TotalNitrogenDensity = TotalNitrogenDensity[i, j],
                    WiltingPoint = WiltingPoint[i, j],
                    Temperature = airdata[i, j].TemperatureAvg,
                    Pressure = airdata[i, j].PressureAvg,
                    Humidity = airdata[i, j].HumidityAvg,
                    ParticulateMatter10 = airdata[i, j].P1Avg,
                    ParticulateMatter2_5 = airdata[i, j].P2Avg
                };


            return results;
        }
    }
}