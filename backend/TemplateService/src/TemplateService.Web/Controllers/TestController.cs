using Microsoft.AspNetCore.Mvc;

namespace TemplateService.Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetInfo()
    {
        return Ok(new
        {
            message = "TestController works!",
        });
    }
}