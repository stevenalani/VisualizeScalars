using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace VisualizeScalars.DataQuery
{
    /// <summary>
    /// Diese Klasse ermöglicht den Download von NASA EarthSearch Informationen.
    /// Damit die Daten zugänglich sind, muss bereits ein EARTHDATA Konto bestehen
    /// </summary>
    public class EarthdataDownloader
    {
        public static Dictionary<string,int> GranulesToLoad = new Dictionary<string, int>
        {
            { "heights",0 },
            { "soils", 0 }
        };
        public readonly string WorkingDirectory;
        private static readonly string urs = "https://urs.earthdata.nasa.gov";
        
        /// <summary>
        /// Dieses dictionary 
        /// </summary>
        public static readonly Dictionary<string,string[]> DataSources = new Dictionary<string, string[]>
        {
            { "heights", new []{ "C1000000240-LPDAAC_ECS","zip" }},
            { "soils", new []{ "C179003650-ORNL_DAAC", "dat" }}

        };
        private string _username;
        private string _password;
        private CookieContainer _cookieContainer = new CookieContainer();
        private CredentialCache cache = new CredentialCache();
        public bool IsLoggedin { get; private set; }

        /// <summary>
        /// Erstelle eine Instanz der Klasse und fordert zur Eingabe der Login-Informationen auf
        /// </summary>
        public EarthdataDownloader(string workingDirectory)
        {
            WorkingDirectory = workingDirectory;
            if (!Directory.Exists(WorkingDirectory))
            {
                Directory.CreateDirectory(WorkingDirectory);
            }
            GetUserInput();
        }
        /// <summary>
        /// Erstelle eine Instanz der Klasse mit Login- Informationen als Parameter
        /// </summary>
        /// <param name="username">Nutzername für EARTH DATA LOGIN</param>
        /// <param name="password">Passwort</param>
        public EarthdataDownloader(string workingDirectory,string username, string password)
        {
            WorkingDirectory = workingDirectory;
            if (!Directory.Exists(WorkingDirectory))
            {
                Directory.CreateDirectory(WorkingDirectory);
            }
            _username = username;
            _password = password;
            cache.Add(new Uri(urs), "Basic", new NetworkCredential(_username, _password));
        }
        /// <summary>
        /// Registriert eine Datenquelle für den Download
        /// </summary>
        /// <param name="key">Name der Quelle</param>
        /// <param name="conceptId">Concept ID des Datensets</param>
        /// <param name="fileFormat">Format in welchem die Dateien zum download stehen</param>
        /// <param name="granuleCount">Anzahl der zu downloadenden Granulare. 0 = Alle Granulare (Vorsicht kann im GB bereich liegen)</param>
        public void RegisterDataSource(string key, string conceptId, string fileFormat ,int granuleCount = 0)
        {
            if (!DataSources.ContainsKey(key))
            {
                DataSources.Add(key, new[] {conceptId, fileFormat});
            }

            if (GranulesToLoad.ContainsKey(key))
            {
                GranulesToLoad.Add(key, granuleCount);
            }
        }

        /// <summary>
        /// Läd die Dateien aller Quellen für die angegebene BoundingBox herunter
        /// </summary>
        /// <param name="latitudeSouth">Unten Longitude</param>
        /// <param name="longitudeWest">Links Latitude</param>
        public void DownloadDataSets(double latitudeSouth, double longitudeWest)
        {
            var keys = DataSources.Keys.Select( key => key.ToString());
            string spatialParam = $"&point={longitudeWest.ToString(CultureInfo.InvariantCulture)},{latitudeSouth.ToString(CultureInfo.InvariantCulture)}";
            foreach (var key in keys)
            {
                DownloadDataSet(key,spatialParam);
            }
        }
        /// <summary>
        /// Läd die Dateien aller Quellen für die angegebene BoundingBox herunter
        /// </summary>
        /// <param name="latitudeSouth">Unten Longitude</param>
        /// <param name="longitudeWest">Links Latitude</param>
        /// <param name="latitudeNorth">Oben Longitude</param>
        /// <param name="longitudeEast">Rechts Latitude</param>
        public void DownloadDataSets(double latitudeSouth, double longitudeWest, double latitudeNorth, double longitudeEast)
        {
            if (latitudeSouth > latitudeNorth)
            {
                var temp = latitudeSouth;
                latitudeSouth = latitudeNorth;
                latitudeNorth = temp;
            }

            if (longitudeWest > longitudeEast)
            {
                var temp = longitudeWest;
                longitudeWest = longitudeEast;
                longitudeEast = temp;
            }
            string spatialParam = $"&bounding_box={latitudeSouth.ToString(CultureInfo.InvariantCulture)},{longitudeWest.ToString(CultureInfo.InvariantCulture)},{latitudeNorth.ToString(CultureInfo.InvariantCulture)},{longitudeEast.ToString(CultureInfo.InvariantCulture)}";
            var keys = DataSources.Keys.Select(key => key.ToString());
            foreach (var key in keys)
            {
                DownloadDataSet(key, spatialParam);
            }
        }
        /// <summary>
        /// Läd die Dateien der angegebenen Quelle für den angegebenen Punkt herunter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitudeSouth"></param>
        /// <param name="longitudeWest"></param>
        public void DownloadDataSet(string key, double latitudeSouth, double longitudeWest)
        {
            string spatialParam = $"&point={latitudeSouth.ToString(CultureInfo.InvariantCulture)},{longitudeWest.ToString(CultureInfo.InvariantCulture)}";
            DownloadDataSet(key, spatialParam);
        }
        /// <summary>
        /// Läd die Dateien der angegebenen Quelle herunter
        /// </summary>
        /// <param name="key"></param>

        public void DownloadDataSet(string key)
        {
            DownloadDataSet(key, "");
        }
        /// <summary>
        /// Läd die Dateien der angegebenen Quelle für die angegebene BoundingBox herunter
        /// </summary>
        /// <param name="key"></param>
        /// <param name="latitudeSouth"></param>
        /// <param name="longitudeWest"></param>
        /// <param name="latitudeNorth">Oben Longitude</param>
        /// <param name="longitudeEast">Rechts Latitude</param>
        public void DownloadDataSet(string key, double latitudeSouth, double longitudeWest, double latitudeNorth, double longitudeEast)
        {
            if (latitudeSouth > latitudeNorth)
            {
                var temp = latitudeSouth;
                latitudeSouth = latitudeNorth;
                latitudeNorth = temp;
            }

            if (longitudeWest > longitudeEast)
            {
                var temp = longitudeWest;
                longitudeWest = longitudeEast;
                longitudeEast = temp;
            }
            string spatialParam = $"&bounding_box={latitudeSouth.ToString(CultureInfo.InvariantCulture)},{longitudeWest.ToString(CultureInfo.InvariantCulture)},{latitudeNorth.ToString(CultureInfo.InvariantCulture)},{longitudeEast.ToString(CultureInfo.InvariantCulture)}";
            DownloadDataSet(key, spatialParam);
        }

        private void DownloadDataSet(string key, string spatialParam)
        {
            var granuleCount = GranulesToLoad[key];
            string url = "https://cmr.earthdata.nasa.gov/search/granules.json?concept_id=" + DataSources[key][0];
            url += spatialParam;
            url += "&sort_key[]=-start_date";
            try
            {
                HttpWebResponse response = getWebResponse(url);
                var collectionInfo = DeserializeCollectionInfoFromResponse(response);
                var dataLinks = collectionInfo.feed.entry.SelectMany(x =>
                    x.links.Where(l => l.rel.EndsWith("/data#") && l.href.EndsWith(DataSources[key][1])));
                if (granuleCount != 0)
                    dataLinks = dataLinks.Take((int)granuleCount);
                foreach (var dataLink in dataLinks)
                {

                    var filename = Path.Combine(WorkingDirectory, Path.GetFileName(dataLink.href));
                    if (!File.Exists(filename))
                    {
                        Console.WriteLine("starting Download for:" + dataLink.href);
                        response = getWebResponse(dataLink.href);
                        long length = response.ContentLength;
                        string type = response.ContentType;
                        DownloadFile(response, filename);
                        Console.WriteLine("done");
                    }
                    else { Console.WriteLine("File already exists"); }

                    var targetDir = Path.Combine(WorkingDirectory, Path.GetFileNameWithoutExtension(filename));
                    if (DataSources[key][1] == "zip" && !Directory.Exists(targetDir))
                    {
                        Console.WriteLine($"Unziping: {filename} into Directory: { targetDir }");
                        UnzipFile(filename, targetDir);
                        Console.WriteLine("done");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Fehler: " + e.Message);
            }
        }



        private void UnzipFile(string filename,string targetDir)
        {
            ZipFile.ExtractToDirectory(filename,targetDir);
        }

        private void DownloadFile(HttpWebResponse response, string filename)
        {
            
            Stream stream = response.GetResponseStream();
            FileStream fs = new FileStream(filename,FileMode.Create);
            stream.CopyTo(fs);
            stream.Close();
            stream.Dispose();
            fs.Close();
            fs.Dispose();
        }
        
        private CollectionInformation DeserializeCollectionInfoFromResponse(WebResponse response)
        {
            Stream stream = response.GetResponseStream();
            string content;
            using (StreamReader reader = new StreamReader(stream))
            {
                content = reader.ReadToEnd();
                reader.Close();
                reader.Dispose();
            }
            return JsonConvert.DeserializeObject<CollectionInformation>(content);
        }

        public void readDatFiles()
        {
            var files = Directory.GetFiles(WorkingDirectory, "*.dat");
            ProfileAvailableWaterCapacityInformation profileAvailableWaterCap = getWaterCapacity(files.First(x => x.Contains("pawc")));
        }

        private ProfileAvailableWaterCapacityInformation getWaterCapacity(string filename)
        {
            StreamReader streamReader = new StreamReader(filename,Encoding.ASCII);
            ProfileAvailableWaterCapacityInformation pwacInfo = new ProfileAvailableWaterCapacityInformation();
            string line = "";
            int lineNo = 1;
            Coordinate coordinate = new Coordinate(-180.0, -56.5000);

            while ((line = streamReader.ReadLine()) != null )
            {
                if (lineNo <= ProfileAvailableWaterCapacityInformation.HeaderRowCount)
                {
                    string[] headerParts = line.Split(' ');
                    pwacInfo.Headers.Add(headerParts[0], headerParts[1]);
                    lineNo++;
                    continue;
                }
                var values = line.Split(' ');
                foreach (var value in values)
                {
                    pwacInfo.Entries.Add(coordinate, new PAWCEntry()
                    {
                        Latitude = coordinate.Latitude,
                        Longitude = coordinate.Longitude,
                        Value = value == ((int)pwacInfo.NoDataValue).ToString()? Convert.ToDouble(value):Double.NaN,
                    });
                    coordinate.Latitude += pwacInfo.CellSize;
                }
                coordinate.Latitude = pwacInfo.FirstCellX;
                coordinate.Longitude += pwacInfo.CellSize;

                lineNo++;
            }

            return pwacInfo;
        }

        private HttpWebResponse getWebResponse(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "GET";
            request.Credentials = cache;
            request.CookieContainer = _cookieContainer;
            request.PreAuthenticate = false;
            request.AllowAutoRedirect = true;
            return (HttpWebResponse)request.GetResponse();
        }
        private void GetUserInput()
        {
            Console.WriteLine("Bitte geben Sie Ihren NASA EarthData Usernamen ein:");
            _username = Console.ReadLine();
            Console.WriteLine("Bitte geben sie das Passwort ein:");
            ConsoleKey key;
            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && _password.Length > 0)
                {
                    Console.Write("\b \b");
                    _password = _password.Remove(_password.Length - 1, 1);
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    _password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);
        }
    }

    public class ProfileAvailableWaterCapacityInformation
    {
        public static readonly int HeaderRowCount = 6;
        public Dictionary<string,string> Headers = new Dictionary<string, string>();
        public int Columns => Headers.ContainsKey("ncols") ? Convert.ToInt32(Headers["ncols"]) : -1;
        public int Rows => Headers.ContainsKey("nrows") ? Convert.ToInt32(Headers["nrows"]) : -1;
        public double CellSize => Headers.ContainsKey("cellsize") ? Convert.ToDouble(Headers["cellsize"],CultureInfo.InvariantCulture) : Double.NaN;
        public double FirstCellX => Headers.ContainsKey("xllcorner") ? Convert.ToDouble(Headers["xllcorner"], CultureInfo.InvariantCulture) : Double.NaN;
        public double FirstCellY => Headers.ContainsKey("yllcorner") ? Convert.ToDouble(Headers["yllcorner"], CultureInfo.InvariantCulture) : Double.NaN;
        public double NoDataValue => Headers.ContainsKey("NODATA_value") ? Convert.ToDouble(Headers["NODATA_value"], CultureInfo.InvariantCulture) : Double.NaN;

        public Dictionary<Coordinate, PAWCEntry> Entries = new Dictionary<Coordinate, PAWCEntry>();

    }

    public struct Coordinate
    {
        public Coordinate(double lat, double lng)
        {
            Latitude = lat;
            Longitude = lng;
        }
        public double Latitude;
        public double Longitude;
    }
    public struct PAWCEntry
    {
        public double Latitude;
        public double Longitude;
        public double Value;
    }

    public class Link
    {
        public string rel { get; set; }
        public string type { get; set; }
        public string title { get; set; }
        public string hreflang { get; set; }
        public string href { get; set; }
        public bool? inherited { get; set; }
    }

    public class Entry
    {
        public string producer_granule_id { get; set; }
        public List<string> boxes { get; set; }
        public DateTime time_start { get; set; }
        public DateTime updated { get; set; }
        public string dataset_id { get; set; }
        public string data_center { get; set; }
        public string title { get; set; }
        public string coordinate_system { get; set; }
        public string day_night_flag { get; set; }
        public DateTime time_end { get; set; }
        public string id { get; set; }
        public string original_format { get; set; }
        public string granule_size { get; set; }
        public bool browse_flag { get; set; }
        public string collection_concept_id { get; set; }
        public bool online_access_flag { get; set; }
        public List<Link> links { get; set; }
    }

    public class Feed
    {
        public DateTime updated { get; set; }
        public string id { get; set; }
        public string title { get; set; }
        public List<Entry> entry { get; set; }
    }

    public class CollectionInformation
    {
        public Feed feed { get; set; }
    }
}
