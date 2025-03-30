using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequestHelper;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/storms")]
public class StormController : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public IActionResult GetCurrentStorms()
    {
        var result = EonetGetCurrentEventsForCategory("severeStorms");
        return Ok(JsonConvert.SerializeObject(result));
    }

    // [HttpGet]
    // [Route("{stormId}")]
    // public IActionResult GetStorm(string stormId)
    // {
    //     
    // }
}