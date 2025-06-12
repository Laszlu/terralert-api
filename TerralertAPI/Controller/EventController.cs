using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using TerralertAPI.Model;
using static TerralertAPI.EonetService;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    private readonly IEonetService _eonetService;
    private readonly IMemoryCache _memoryCache;

    public EventController(IEonetService eonetService, IMemoryCache memoryCache)
    {
        _eonetService = eonetService;
        _memoryCache = memoryCache;
    }
    
    [HttpGet]
    [Route("{category}/current")]
    public async Task<IActionResult> GetCurrentStorms(string category)
    {
        var eonetCategory = _eonetService.ConvertEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }

        if (_memoryCache.TryGetValue("currentEvents" + eonetCategory, out List<Event>? events))
        {
            return Ok(events);
        }
        
        var result = await _eonetService.EonetGetCurrentEventsForCategory(eonetCategory);

        var cacheOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(6)
        };
        
        _memoryCache.Set("currentEvents" + eonetCategory, result, cacheOptions);
        
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Route("{category}/{eventId}")]
    public async Task<IActionResult> GetStorm(string category, string eventId)
    {
        var eonetCategory = _eonetService.ConvertEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }
        
        if (_memoryCache.TryGetValue("currentEvents" + eonetCategory, out List<Event>? events))
        {
            if (events != null && events.Any(e => e.Id == eventId))
            {
                return Ok(events.First(e => e.Id == eventId));
            }
        }
        
        var result = await _eonetService.EonetGetEventById(eventId);
        
        return Ok(JsonConvert.SerializeObject(result));
    }
}