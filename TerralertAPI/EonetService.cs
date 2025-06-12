using System.Diagnostics;
using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public class EonetService : IEonetService
{
    public string? ConvertEventCategory(string requestCategory)
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
    
    public async Task<List<Event>?> EonetGetCurrentEventsForCategory(string category)
    {
        using var client = new HttpClient();

        var result = await client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?category={category}&status=open");

        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        var response = JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);

        return response?.Events;
    }

    public async Task<Event?> EonetGetEventById(string id)
    {
        using var client = new HttpClient();
        
        var result = await client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events/{id}");
        
        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<Event>(jsonString);
    }
}