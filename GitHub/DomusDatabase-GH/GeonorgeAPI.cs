using System.Net;
using Newtonsoft.Json.Linq;
using DomusDatabase.Helpers;

namespace Domus.Geonorge
{
    ///<summary>
    ///API interface for Geonorge API
    ///</summary>
    public class GeonorgeAPI
    {
        public static Dictionary<string, float> GetGeonorgeCoords(string url, string address) 
        {
            HttpClient client = new();

            // Processing address. Move to sep method inside Scraper.
            if (address.Contains("-"))
            {
                address = address.Split("-")[0];
            }
            else if (address.Count<char>(c => c == ',') > 2)
            {
                var adr = address.Split(address.First(c => c == ','))[0];
                System.Console.WriteLine(adr);
                var kommune = address.Split(address.Last(c => c == ','))[1];

                address = adr + kommune;
                System.Console.WriteLine(address);
            }
            if (address.Contains(','))
            {
                address = address.Remove(address.IndexOf(','), 1);
            }
            // System.Console.WriteLine(address); //Add log call
            //--------------------------------------------------------

            //Api call URL
            string URI = url + $"sok={address}&fuzzy=true";
            
            //Call api
            var res = client.GetAsync(URI).Result; 
            var json = res.Content.ReadAsStringAsync().Result;
            //Add log call

            //DEBUGGING
            //System.Console.WriteLine(json);
            //DEBUGGING
            

            //Create sep method
            //Parse json from API call, extract latitude and longitude pos data.
            var coords = new Dictionary<string, float>();
            try
            {
                JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(json);
                
                var lat = (float)jObject["adresser"][0]["representasjonspunkt"]["lat"]; //Unacceptable
                var lon = (float)jObject["adresser"][0]["representasjonspunkt"]["lat"]; // ----||----

                coords["lat"] = lat;
                coords["lon"] = lon;
            }
            catch
            {
                coords["lat"] = 0;
                coords["lon"] = 0;
            } 
            //----------------------------------------------


            return coords;
        }
    }

}