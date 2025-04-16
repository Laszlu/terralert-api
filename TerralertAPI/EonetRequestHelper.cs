using System.Diagnostics;
using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public static class EonetRequestHelper
{
    public static string? ConvertEventCategory(string requestCategory)
    {
        return requestCategory switch
        {
            "st" => "severeStorms",
            "wi" => "wildfires",
            "vo" => "volcanoes",
            "ea" => "earthquakes",
            _ => null
        };
    }
    
    public static EonetEventListResult? EonetGetCurrentEventsForCategory(string category)
    {
        using var client = new HttpClient();

        var result = client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?category={category}&status=open");

        var jsonString = result.Result.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);
    }

    public static Event? EonetGetEventById(string id)
    {
        using var client = new HttpClient();
        
        var result = client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events/{id}");
        
        var jsonString = result.Result.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<Event>(jsonString);
    }
}