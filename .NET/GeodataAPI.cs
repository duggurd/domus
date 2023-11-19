using System.Net;
using Newtonsoft.Json.Linq;

namespace Domus.GeodataAPI
{
    public class GeodataAPI
    {
        public static string GetAddressInfo(string address)
        {
            HttpClient client = new();
            string URI = AppConfig.Config.GEODATA_API_URL + $"sok={address}";
            
            var res = client.GetAsync(URI).Result; 
            return res.Content.ReadAsStringAsync().Result;
        }

        public static Dictionary<string, float> ExtractCoords(string json)
        {
            JObject? deserialized = Newtonsoft.Json.JsonConvert.DeserializeObject<JObject>(json);
            var coords = new Dictionary<string, float>() 
            {
                {"lat", (float)deserialized["adresser"][1]["representasjonspunkt"]["lat"]},
                {"lon", (float)deserialized["adresser"][1]["representasjonspunkt"]["lon"]}
            };
            return coords;
            
            
        }
    }

}