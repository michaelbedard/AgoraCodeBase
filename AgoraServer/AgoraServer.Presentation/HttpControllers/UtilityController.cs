using Agora.Core;
using Application.Handlers.Utility.GetAllSessions;
using Domain.Entities.Runtime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class UtilityController : ControllerBase
{
    private readonly IMediator _mediator;

    public UtilityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet("version")]
    public async Task<ActionResult<string>> GetVersion()
    {
        return Ok(Constants.Version);
    }
    
    [HttpGet("connectedUsers")]
    public async Task<ActionResult<List<RuntimeUser>>> GetConnectedUsers()
    {
        var query = new GetConnectedUsersQuery();
        var result = await _mediator.Send(query);
        
        return result.IsSuccess ? Ok(JsonConvert.SerializeObject(result.Value)) : BadRequest(result.Error);
    }
}