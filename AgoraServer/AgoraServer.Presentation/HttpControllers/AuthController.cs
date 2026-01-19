using Agora.Core.Dtos;
using Agora.Core.Payloads.Http.Auth;
using Application.Handlers.Auth.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Presentation.Controllers.Other;
using LoginRequest = Application.Handlers.Auth.Login.LoginRequest;

namespace Presentation.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : BaseApiController
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    // Login route
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginPayload loginPayload)
    {
        return Ok(new UserDto()
        {
            Id = "an_id",
            Username = "Marc",
            LobbyId = "009",
            Avatar = 2,
            Pronouns = 3
        });
            
        var command = new LoginRequest
        {
            OAuthCode = loginPayload.OAuthCode,
        };
        
        var result = await _mediator.Send(command);
        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }
    
    // Logout route
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var userResult = GetCurrentUser();
        if (!userResult.IsSuccess) return BadRequest(userResult.Error);
        
        var request = new LogoutRequest()
        {
            User = userResult.Value,
        };
        
        var result = await _mediator.Send(request);
        return result.IsSuccess ? Ok() : BadRequest(result.Error);
    }
}