using Microsoft.AspNetCore.Mvc;

namespace Presentation.HttpControllers.Other;

[ApiController]
[Route("[controller]")]
public class HttpTestController : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> TestGet()
    {
        return Ok("GET requests work!");
    }
    
    [HttpPost]
    public async Task<IActionResult> TestPost(object data)
    {
        return Ok("POST requests work");
    }
    
    [HttpDelete]
    public async Task<IActionResult> TestDelete()
    {
        return Ok("DELETE requests work");
    }
    
    [HttpPut]
    public async Task<IActionResult> TestPut(object data)
    {
        return Ok("PUT requests work");
    }
}