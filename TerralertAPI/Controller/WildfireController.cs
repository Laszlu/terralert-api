using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequestHelper;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/wildfires")]
public class WildfireController : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public IActionResult GetCurrentEarthquakes()
    {
        var result = EonetGetCurrentEventsForCategory("wildfires");
        return Ok(JsonConvert.SerializeObject(result));
    }
}
