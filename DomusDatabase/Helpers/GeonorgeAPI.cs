using System.Net;
using Newtonsoft.Json.Linq;

namespace Domus
{
    ///<summary>
    ///API interface for Geonorge API
    ///</summary>
    public class GeonorgeAPI
    {
        public static Dictionary<string, float> GetGeonorgeCoords(string url, string address, ILogger? logger = null) 
        {
            HttpClient client = new();

            //Api call URL
            string URI = url + $"sok={address}&fuzzy=true";
            
            //Call api
            var res = client.GetAsync(URI).Result; 
            var json = res.Content.ReadAsStringAsync().Result;
            if (logger != null) {logger.LogDebug("Json recieved from API call: {0}", json);}

            return ExtractCoordsFromJson(json);
        }

        ///<summary>
        ///Extracts longitude and latitude position data from JSON
        ///</summary>
        private static Dictionary<string, float> ExtractCoordsFromJson(string json)
        {
            var coords = new Dictionary<string, float>();
            try
            {
                JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                
                var lat = (float)jObject["adresser"][0]["representasjonspunkt"]["lat"]; //Unacceptable
                var lon = (float)jObject["adresser"][0]["representasjonspunkt"]["lon"]; // ----||----
                
                coords["lat"] = lat;
                coords["lon"] = lon;
            }
            
            catch
            {
                coords["lat"] = 0;
                coords["lon"] = 0;
            } 

            return coords;
        }
    }
}