using Agora.Core.Dtos;
using Agora.Core.Payloads.Http.Auth;
using Application.Handlers.Auth.Logout;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.HttpControllers.Other;
using LoginRequest = Application.Handlers.Auth.Login.LoginRequest;

namespace Presentation.HttpControllers;

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
        var command = new LoginRequest
        {
            OAuthCode = loginPayload.OAuthCode,
            ChannelId = loginPayload.ChannelId,
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