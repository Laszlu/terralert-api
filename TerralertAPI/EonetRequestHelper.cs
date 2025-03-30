using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public static class EonetRequestHelper
{
    public static EonetEventListResult? EonetGetCurrentEventsForCategory(string category)
    {
        using var client = new HttpClient();

        var result = client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?category={category}&status=open");

        var jsonString = result.Result.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);
    }
}