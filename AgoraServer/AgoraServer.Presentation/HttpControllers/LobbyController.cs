using Agora.Core.Dtos.Lobby;
using Application.Handlers.Lobby.GetLobby;
using Application.Handlers.Lobby.JoinLobby;
using Application.Handlers.Lobby.LeaveLobby;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.HttpControllers.Other;

namespace Presentation.HttpControllers;

public class LobbyController : BaseApiController
{
    private readonly IMediator _mediator;

    public LobbyController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    // POST /Lobby/join?userId=Steve
    [HttpPost("join")]
    public async Task<ActionResult<LobbyDto>> JoinLobby([FromBody] string lobbyId)
    {
        // 1. Get User from Base
        var userResult = GetCurrentUser();
        if (!userResult.IsSuccess) return BadRequest(userResult.Error);

        // 2. Use User
        var request = new JoinLobbyRequest
        {
            User = userResult.Value,
            LobbyId = lobbyId
        };
        
        var result = await _mediator.Send(request);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    // POST /Lobby/leave?userId=Steve
    [HttpPost("leave")]
    public async Task<IActionResult> LeaveLobby([FromBody] string lobbyId)
    {
        var userResult = GetCurrentUser();
        if (!userResult.IsSuccess) return BadRequest(userResult.Error);

        var request = new LeaveLobbyRequest
        {
            User = userResult.Value,
            LobbyId = lobbyId
        };
        
        var result = await _mediator.Send(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
    
    // GET /Lobby/Room1
    [HttpGet("{lobbyId}")]
    public async Task<ActionResult<LobbyDto>> GetLobby(string lobbyId)
    {
        var query = new GetLobbyQuery { Id = lobbyId };
        
        var result = await _mediator.Send(query);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
}