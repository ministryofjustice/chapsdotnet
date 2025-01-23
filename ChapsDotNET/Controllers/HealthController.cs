using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChapsDotNET.Controllers;

[ApiController]
[Route("dotnet-health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    [AllowAnonymous]
    public IActionResult GetHealth()
    {
        return Ok("Healthy");
    }
}
