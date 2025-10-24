using System.Diagnostics;
using Newtonsoft.Json;
using TerralertAPI.Helper;
using TerralertAPI.Model;

namespace TerralertAPI;

public interface IEonetService
{
    public EventCategory? ParseEventCategory(string requestCategory);

    public Region? ParseRegion(string requestRegion);

    public Task<List<ResponseEvent>?> EonetGetCurrentEventsForCategory(string category);

    public Task<ResponseEvent?> EonetGetEventById(string id);

    public Task<List<ResponseEvent>?> EonetGetEventsForCategoryRegionAndYear(EventCategory category, Region region, int year);
}

public class EonetService : IEonetService
{
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
    
    public async Task<List<ResponseEvent>?> EonetGetCurrentEventsForCategory(string category)
    {
        using var client = new HttpClient();
        
        var result = await client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events?category={category}&status=open");

        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        var eventList = JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);

        if (eventList?.Events == null) return null;

        List<ResponseEvent> responseEvents = [];
        
        foreach (var eventListEvent in eventList.Events)
        {
            try
            {
                responseEvents.Add(EventConversionHelper.ConvertEonetEvent(eventListEvent));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }
            
        return responseEvents;
    }

    public async Task<ResponseEvent?> EonetGetEventById(string id)
    {
        using var client = new HttpClient();
        
        var result = await client.GetAsync($"https://eonet.gsfc.nasa.gov/api/v3/events/{id}");
        
        var jsonString = result.Content.ReadAsStringAsync().Result;
        
        var eonetEvent = JsonConvert.DeserializeObject<EonetEvent>(jsonString);
        
        if (eonetEvent == null) return null;
        
        var responseEvent = EventConversionHelper.ConvertEonetEvent(eonetEvent);
        
        return responseEvent;
    }
    
    public async Task<List<ResponseEvent>?> EonetGetEventsForCategoryRegionAndYear(EventCategory category, Region region, int year)
    {
        using var client = new HttpClient();
        
        var categoryString = $"category={category.FullCategoryString}";
        var regionString = $"bbox={region.MinLongitude},{region.MaxLatitude},{region.MaxLongitude},{region.MinLatitude}";
        var yearString = $"start={year}-01-01&end={year}-12-31";

        var message = new HttpRequestMessage(HttpMethod.Get,
            $"https://eonet.gsfc.nasa.gov/api/v3/events?{categoryString}&{regionString}&{yearString}&status=all") {Version = new Version(2, 0)};
        
        var result = await client.SendAsync(message);
        
        var jsonString = result.Content.ReadAsStringAsync().Result;

        EonetEventListResult? eventList;
        
        try
        {
            eventList = JsonConvert.DeserializeObject<EonetEventListResult>(jsonString);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        
        if (eventList?.Events == null) return null;

        List<ResponseEvent> responseEvents = [];
        
        foreach (var eventListEvent in eventList.Events)
        {
            responseEvents.Add(EventConversionHelper.ConvertEonetEvent(eventListEvent));
        }
            
        return responseEvents;
    }
}