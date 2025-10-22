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
    
    private MemoryCacheEntryOptions _entryOptions;

    public EventController(IEonetService eonetService, IMemoryCache memoryCache)
    {
        _eonetService = eonetService;
        _memoryCache = memoryCache;

        _entryOptions = new MemoryCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(24)
        };
    }
    
    [HttpGet]
    [Route("{category}/current")]
    public async Task<IActionResult> GetCurrentEventsByCategory(string category)
    {
        var eonetCategory = _eonetService.ParseEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }

        if (_memoryCache.TryGetValue("currentEvents_" + eonetCategory.FullCategoryString, out List<Event>? events))
        {
            return Ok(JsonConvert.SerializeObject(events));
        }
        
        var result = await _eonetService.EonetGetCurrentEventsForCategory(eonetCategory.FullCategoryString);

        if (result != null && result.Count != 0)
        {
            _memoryCache.Set("currentEvents_" + eonetCategory.FullCategoryString, result, _entryOptions);
        }

        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Route("{category}/{eventId}")]
    public async Task<IActionResult> GetSingleEventByCategory(string category, string eventId)
    {
        var eonetCategory = _eonetService.ParseEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }

        foreach (var eventCategory in EventCategoryMapper.AllCategories)
        {
            if (_memoryCache.TryGetValue("currentEvents_" + eventCategory.FullCategoryString, out List<Event>? events))
            {
                if (events != null && events.Any(e => e.Id == eventId))
                {
                    return Ok(JsonConvert.SerializeObject(events.First(e => e.Id == eventId)));
                }
            }
        }
            
        var result = await _eonetService.EonetGetEventById(eventId);
        
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Route("{category}/{region}/{year}")]
    public async Task<IActionResult> GetEventsByCategoryRegionAndYear(string category, string region, string year)
    {
        var eonetCategory = _eonetService.ParseEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }

        var requestedRegion = _eonetService.ParseRegion(region);
        if (requestedRegion == null)
        {
            return BadRequest($"Invalid region requested: {region}");
        }

        if (!int.TryParse(year, out var requestedYear))
        {
            return BadRequest($"Invalid year requested: {year}");
        }

        var currentYear = DateTime.Now.Year;
        if (requestedYear < currentYear - 10)
        {
            return BadRequest($"Requested year ({requestedYear}) is outside of tracked timeframe");
        }

        if (_memoryCache.TryGetValue($"events_{eonetCategory.FullCategoryString}_{region}_{year}", out List<Event>? events))
        {
            return Ok(JsonConvert.SerializeObject(events));
        }

        var results = await _eonetService.EonetGetEventsForCategoryRegionAndYear(eonetCategory, requestedRegion, requestedYear);

        if (results != null && results.Count != 0)
        {
            _memoryCache.Set($"events_{eonetCategory.FullCategoryString}_{region}_{year}", results, _entryOptions);
        }
        
        return Ok(JsonConvert.SerializeObject(results));
    }
}