using System.Globalization;
using System.IO;
using System.Net;

namespace VisualizeScalars.DataQuery
{
    public class OverpassApiQuerent
    {
        private string querystring(double lat1, double lng1, double lat2, double lng2) =>
            $"https://overpass-api.de/api/interpreter?data=[out:json];" +
            $"(node({lat1.ToString(CultureInfo.InvariantCulture)},{lng1.ToString(CultureInfo.InvariantCulture)},{lat2.ToString(CultureInfo.InvariantCulture)},{lng2.ToString(CultureInfo.InvariantCulture)});" +
            $"relation({lat1.ToString(CultureInfo.InvariantCulture)},{lng1.ToString(CultureInfo.InvariantCulture)},{lat2.ToString(CultureInfo.InvariantCulture)},{lng2.ToString(CultureInfo.InvariantCulture)}); );" +
            $"(._; <;);out;";
        public void QueryBoundingBox(double lat1, double lng1, double lat2, double lng2)
        {
            WebClient client = new WebClient();
            client.BaseAddress = "https://overpass-api.de";
            var result = client.DownloadString(querystring(lat1, lng1, lat2, lng2));
            File.WriteAllText($"D:\\earthdata\\osm-{lat1.ToString().Replace(",","_")}_{lng1.ToString().Replace(",","_")}_to_{lat2.ToString().Replace(",", "_")}_{lng2.ToString().Replace(",", "_")}.json",result);
        }
    }
}
