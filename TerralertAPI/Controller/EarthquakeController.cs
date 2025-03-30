using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequestHelper;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/earthquakes")]
public class EarthquakeController : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public IActionResult GetCurrentEarthquakes()
    {
        var result = EonetGetCurrentEventsForCategory("earthquakes");
        return Ok(JsonConvert.SerializeObject(result));
    }
}