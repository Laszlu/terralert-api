using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static TerralertAPI.EonetRequest;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/storms")]
public class StormApiController : ControllerBase
{
    [HttpGet]
    [Route("current")]
    public IActionResult GetCurrentStorms()
    {
        var result = GetCurrentStormsEonet();
        return Ok(JsonConvert.SerializeObject(result));
    }
}