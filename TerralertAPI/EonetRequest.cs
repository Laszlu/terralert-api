using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public static class EonetRequest
{
    public static EonetCategoryResult? GetCurrentStormsEonet()
    {
        using var client = new HttpClient();

        var result = client.GetAsync("https://eonet.gsfc.nasa.gov/api/v3/events?category=severeStorms&status=open");

        var jsonString = result.Result.Content.ReadAsStringAsync().Result;
        
        var storms = JsonConvert.DeserializeObject<EonetCategoryResult>(jsonString);
        
        return storms;
    }
}