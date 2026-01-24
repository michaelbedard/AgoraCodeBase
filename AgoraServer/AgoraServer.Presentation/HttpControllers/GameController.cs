using Agora.Core.Payloads.Hubs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.HttpControllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IMediator _mediator;

    public GameController(IMediator mediator)
    {
        _mediator = mediator;
    }
}