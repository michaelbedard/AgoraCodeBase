using Agora.Core;
using Application.Handlers.Utility.GetConnectedUsers;
using Domain.Entities.Runtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.HttpControllers;

[ApiController]
[Route("[controller]")]
public class UtilityController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IConfiguration _config;

    public UtilityController(IMediator mediator, IConfiguration configuration)
    {
        _mediator = mediator;
        _config = configuration;
    }
    
    [HttpGet("version")]
    public async Task<ActionResult<string>> GetVersion()
    {
        return Ok(Constants.Version);
    }
    
    [HttpGet("test")]
    public async Task<ActionResult<string>> GetTest()
    {
        var clientId = _config["Discord:ClientId"]!;
        var clientSecret = Environment.GetEnvironmentVariable("DISCORD_CLIENT_SECRET");
            
        return Ok(clientId + "::" + clientSecret);
    }
    
    [HttpGet("connectedUsers")]
    public async Task<ActionResult<List<RuntimeUser>>> GetConnectedUsers()
    {
        var query = new GetConnectedUsersQuery();
        var result = await _mediator.Send(query);
        
        return result.IsSuccess ? Ok(JsonConvert.SerializeObject(result.Value)) : BadRequest(result.Error);
    }
}