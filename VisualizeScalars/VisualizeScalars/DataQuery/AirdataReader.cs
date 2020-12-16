﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Newtonsoft.Json;

namespace VisualizeScalars.DataQuery
{
    class AirdataReader
    {
        private readonly string _workingDirectory;
        private readonly string _airdatapath = "luftdaten.info";
        private readonly Uri apiUri = new Uri("http://api.luftdaten.info/static/v1/data.json");
        private string datadirectory => Path.Combine(_workingDirectory, _airdatapath);
        public AirdataReader(string workingDirectory)
        {
            _workingDirectory = workingDirectory;
        }

        public void DownloadData()
        {
            var fileInfos = new DirectoryInfo(datadirectory).EnumerateFiles("*.json");
            var lastDownload = fileInfos.OrderBy(x => x.CreationTime).LastOrDefault();
            if (lastDownload == null || lastDownload.CreationTime < DateTime.Now.AddMinutes(-5))
            {
                HttpWebResponse response = getWebResponse(apiUri);
                SafeJson(response);
            }
        }
        private HttpWebResponse getWebResponse(Uri url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.PreAuthenticate = false;
            request.AllowAutoRedirect = true;
            return (HttpWebResponse)request.GetResponse();
        }

        private void SafeJson(HttpWebResponse response)
        {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var content = reader.ReadToEnd();
                    File.WriteAllText(Path.Combine(datadirectory,$"data{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}_{DateTime.Now.Hour}_{DateTime.Now.Minute}.json"), content);
                    reader.Close();
                    reader.Dispose();
                }
        }
        private List<AirSensorData> Deserialize(string content)
        {

            return JsonConvert.DeserializeObject<List<AirSensorData>>(content,new JsonSerializerSettings()
            {
                FloatFormatHandling = FloatFormatHandling.DefaultValue,
                Error = (sender, args) =>
                {
                    if (args.CurrentObject is Sensordatavalue)
                    {
                        Sensordatavalue value = args.CurrentObject as Sensordatavalue;

                        if (value != null && args.ErrorContext.Member.ToString() == "value")
                        {
                            value.value = double.NaN;
                            args.ErrorContext.Handled = true;
                        }
                    }else if (args.CurrentObject is Location)
                    {
                        Location location = args.CurrentObject as Location;

                        if (location != null && args.ErrorContext.Member.ToString() == "latitude")
                        {
                            location.latitude = double.NaN;
                            args.ErrorContext.Handled = true;
                        }
                        if (location != null && args.ErrorContext.Member.ToString() == "longitude")
                        {
                            location.longitude = double.NaN;
                            args.ErrorContext.Handled = true;
                        }
                    }
                }

            });
        }

        public AirInformation[,] BuildGrid(double latitudeSouth, double longitudeWest, double latitudeNorth, double longitudeEast, double gridUnit)
        {
            var cellCountX = (int)((longitudeEast - longitudeWest) / gridUnit);
            var cellCountY = (int)((latitudeNorth - latitudeSouth) / gridUnit);
            AirInformation[,] result = new AirInformation[cellCountX,cellCountY];
            
            var files = Directory.EnumerateFiles(datadirectory,"*.json");
            List<AirSensorData> temp = new List<AirSensorData>();
            foreach (var file in files)
            {
                var content = File.ReadAllText(file);
                temp.AddRange(Deserialize(content).Where(x => 
                    x.location.latitude <= latitudeNorth && x.location.latitude >= latitudeSouth &&
                    x.location.longitude <= longitudeEast && x.location.longitude >= longitudeWest));
            }

            for (int y = 0; y < cellCountY; y++)
            {
                var lat = latitudeNorth - y * gridUnit;
                for (int x = 0; x < cellCountX; x++)
                {
                    result[x, y] = new AirInformation();
                    var lng = longitudeEast - x * gridUnit;
                    var values = temp.Where(x => Math.Abs(x.location.latitude - lat) < gridUnit && Math.Abs(x.location.longitude - lng) < gridUnit).SelectMany(d => d.sensordatavalues);
                    
                    if (values.Count() > 0)
                    {
                        /*var particularMatterData = values.Where(v =>
                        v.sensordatavalues.All(d => d.value_type == "P1" || d.value_type == "P2"));
                        var climateData = values.Where(v =>
                        v.sensordatavalues.All(d => d.value_type == "temperature" || d.value_type == "pressure" || d.value_type == "humidity"));*/
                        List<double> p1 = new List<double>();
                        List<double> p2 = new List<double>();
                        List<double> temperature = new List<double>();
                        List<double> humidity = new List<double>();
                        List<double> pressure = new List<double>();
                        foreach (var data in values)
                        {
                            switch (data.value_type)
                            {
                                case "P1": 
                                    p1.Add(data.value);
                                    break;
                                case "P2": 
                                    p2.Add(data.value);
                                    break;
                                case "temperature":
                                    temperature.Add(data.value);
                                    break;
                                case "pressure":
                                    pressure.Add(data.value);
                                    break;
                                case "humidity":
                                    humidity.Add(data.value);
                                    break;
                                default: break;

                            }
                        }

                        AirInformation ai = new AirInformation
                        {
                            P1 = p1.ToArray(),
                            P2 = p2.ToArray(),
                            Temperature = temperature.ToArray(),
                            Pressure = pressure.ToArray(),
                            Humidity = humidity.ToArray(),

                        };
                        result[x, y] = ai;
                    }
                }
            }
            return result;
        }
    }

    public class Location
    {
        public string id { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public string altitude { get; set; }
        public string country { get; set; }
        public int exact_location { get; set; }
        public int indoor { get; set; }
    }

    public class SensorType
    {
        public string id { get; set; }
        public string name { get; set; }
        public string manufacturer { get; set; }
    }

    public class Sensor
    {
        public string id { get; set; }
        public string pin { get; set; }
        public SensorType sensor_type { get; set; }
    }

    public class Sensordatavalue
    {
        public string id { get; set; }
        public double value { get; set; }
        public string value_type { get; set; }
    }

    public class AirSensorData
    {
        public string id { get; set; }
        public string sampling_rate { get; set; }
        public DateTime timestamp { get; set; }
        public Location location { get; set; }
        public Sensor sensor { get; set; }
        public List<Sensordatavalue> sensordatavalues { get; set; }
    }

    public class AirInformation
    {
        public double[] P1 { get; set; }
        public double[] P2 { get; set; }
        public double[] Temperature { get; set; }
        public double[] Pressure { get; set; }
        public double[] Humidity { get; set; }
        public double P1Avg => P1 != null ? P1.Sum() / P1.Length : double.NaN;
        public double P2Avg => P2 != null ? P2.Sum() / P2.Length : double.NaN;
        public double TemperatureAvg => Temperature!=null? Temperature.Sum() / Temperature.Length : double.NaN;
        public double PressureAvg => Pressure != null ? Pressure.Sum() / Pressure.Length : double.NaN;
        public double HumidityAvg => Humidity != null ? Humidity.Sum() / Humidity.Length : double.NaN;
    }
}