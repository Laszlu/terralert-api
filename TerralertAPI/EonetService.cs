using System.Diagnostics;
using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public interface IEonetService
{
    public string? ConvertEventCategory(string requestCategory);

    public Task<List<Event>?> EonetGetCurrentEventsForCategory(string category);

    public Task<Event?> EonetGetEventById(string id);
}

public class EonetService : IEonetService
{
    public string? ConvertEventCategory(string requestCategory)
    {
        var category = EventCategoryMapper.FromCode(requestCategory);

        return category?.FullCategoryString;
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