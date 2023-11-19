using Newtonsoft.Json.Linq;

namespace DomusRefactor;

public class GeonorgeApi {
    public static Coordinate GetAddressCoordinates(string address) {
        var json = GetAddressInfoFromApi(address);
        return ExtractCoordinatesFromJson(json);
    }

    private Coordinate ExtractCoordinatesFromJson(string json) {
        var jObject = JObject.Parse(json);

         Coordinate coordinate = new() {
            latitude = ExtractLatitude(jObject),
            longitude = ExtractLongitude(jObject)
        };
        return coordinate;
    }

    private float ExtractLatitude(JObject jObject) {
        return (float)jObject["adresser"][0]["representasjonspunkt"]["lat"];
    }

    private float ExtractLongitude(JObject jObject) {
        return (float)jObject["adresser"][0]["representasjonspunkt"]["lon"];
    }

    private string GetAddressInfoFromApi(string address) {
        var apiCallUrl = ConstructGeonorgeApiCall(address);
        return GetJsonDataFromApi(apiCallUrl);
    }

    private string ConstructGeonorgeApiCall(string address) {
        return $"https://ws.geonorge.no/adresser/v1/sok?sok={address}&fuzzy=true";
    }
    private string GetJsonDataFromApi(string apiUrl) {
        HttpClient client = new();
        return client.GetAsync(apiUrl)
            .Result.Content
            .ReadAsStringAsync().Result;
    }
}

public class Coordinate {
    public float longitude { get; set; }
    public float latitude { get; set; }
}