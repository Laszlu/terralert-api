using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequestHelper;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/volcanoes")]
public class VolcanoeController : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public IActionResult GetCurrentVolcanoes()
    {
        var result = EonetGetCurrentEventsForCategory("volcanoes");
        return Ok(JsonConvert.SerializeObject(result));
    }
}