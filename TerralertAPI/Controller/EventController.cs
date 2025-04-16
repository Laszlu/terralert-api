using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequestHelper;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/events")]
public class EventController : ControllerBase
{
    [HttpGet]
    [Route("{category}/current")]
    public IActionResult GetCurrentStorms(string category)
    {
        var eonetCategory = ConvertEventCategory(category);
        if (eonetCategory == null)
        {
            return BadRequest($"Invalid category requested: {category}");
        }
        var result = EonetGetCurrentEventsForCategory(eonetCategory);
        return Ok(JsonConvert.SerializeObject(result));
    }

    [HttpGet]
    [Route("{eventId}")]
    public IActionResult GetStorm(string eventId)
    {
        var result = EonetGetEventById(eventId);
        return Ok(JsonConvert.SerializeObject(result));
    }
}