using System.Diagnostics;
using Newtonsoft.Json;
using TerralertAPI.Model;

namespace TerralertAPI;

public interface IEonetService
{
    public EventCategory? ParseEventCategory(string requestCategory);

    public Region? ParseRegion(string requestRegion);

    public Task<List<Event>?> EonetGetCurrentEventsForCategory(string category);

    public Task<Event?> EonetGetEventById(string id);

    public Task<List<Event>?> EonetGetEventsForCategoryRegionAndYear(EventCategory category, Region region, int year);
}

public class EonetService : IEonetService
{
    private readonly HttpClient _httpClient;

    public EonetService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public EventCategory? ParseEventCategory(string requestCategory)
    {
        var category = EventCategoryMapper.FromCode(requestCategory);

        return category;
    }

    public Region? ParseRegion(string requestRegion)
    {
        var region = RegionCoordinateMapper.FromNameString(requestRegion);

        return region;
    }
    
    public async Task<List<Event>?> EonetGetCurrentEventsForCategory(string category)
    {
        var result = await _httpClient.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?category={category}&status=open");

        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        var response = JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);

        return response?.Events;
    }

    public async Task<Event?> EonetGetEventById(string id)
    {
        var result = await _httpClient.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events/{id}");
        
        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        return JsonConvert.DeserializeObject<Event>(jsonString);
    }
    
    public async Task<List<Event>?> EonetGetEventsForCategoryRegionAndYear(EventCategory category, Region region, int year)
    {
        var categoryString = $"category={category.FullCategoryString}";
        var regionString = $"bbox={region.MinLongitude},{region.MaxLatitude},{region.MaxLongitude},{region.MinLatitude}";
        var yearString = $"start={year}-01-01&end={year}-12-31";
        
        var result = await _httpClient.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?{categoryString}&{regionString}&{yearString}&status=all");
        
        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        var response = JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);
        
        return response?.Events;
    }
}