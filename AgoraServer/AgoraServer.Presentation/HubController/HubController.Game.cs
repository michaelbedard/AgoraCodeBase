using Agora.Core.Contracts.Server;
using Agora.Core.Payloads.Http.Lobby;
using Agora.Core.Payloads.Hubs;
using Application.Handlers.Game.ExecuteAction;
using Application.Handlers.Game.ExecuteInput;
using Application.Handlers.Lobby.LaunchGame;
using Application.Handlers.Lobby.SelectGame;

namespace Presentation.HubController;

public partial class HubController
{
    /// <summary>
    /// Select, Launch, SetIsReadyToStart
    /// </summary>
    
    public async Task SelectGame(SelectGamePayload payload)
    {
        var request = new SelectGameRequest()
        {
            User = GetClientFromConnection(),
            GameKey = payload.GameKey,
        };
        
        var result = await _mediator.Send(request);
        await SendErrorIfNotSuccessful(result);
    }
    
    public async Task LaunchGame(LaunchGamePayload payload)
    {
        var request = new LaunchGameRequest()
        {
            User = GetClientFromConnection(),
            GameKey = payload.GameKey,
        };
        
        var result = await _mediator.Send(request);
        await SendErrorIfNotSuccessful(result);
    }
    
    public async Task SetIsReadyToStart(ExecuteActionPayload payload)
    {
        // TODO
    }
    
    /// <summary>
    ///  Execute Action and Inputs
    /// </summary>
    
    public async Task ExecuteAction(ExecuteActionPayload payload)
    {
        var request = new ExecuteActionRequest()
        {
            User = GetClientFromConnection(),
            ActionId = payload.ActionId
        };
        
        var result = await _mediator.Send(request);
        await SendErrorIfNotSuccessful(result);
    }
    
    public async Task ExecuteInput(ExecuteInputPayload payload)
    {
        var request = new ExecuteInputRequest()
        {
            User = GetClientFromConnection(),
            InputId = payload.InputId,
            Input = payload.Answer
        };
        
        var result = await _mediator.Send(request);
        await SendErrorIfNotSuccessful(result);
    }
}