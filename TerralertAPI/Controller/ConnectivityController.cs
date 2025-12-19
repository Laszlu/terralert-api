using Microsoft.AspNetCore.Mvc;

namespace TerralertAPI.Controller;

[ApiController]
[Route("api/connection")]
public class ConnectivityController : ControllerBase
{
    [HttpGet]
    [Route("version")]
    public Task<IActionResult> GetVersion()
    {
        return Task.FromResult<IActionResult>(Ok("Version: 1.0.0"));
    }
}