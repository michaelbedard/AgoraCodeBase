using Agora.Core.Payloads.Hubs;
using Application.Handlers.Game.ExecuteAction;
using Application.Handlers.Game.ExecuteInput;

namespace Presentation.HubController;

public partial class HubController
{
    public async Task ExecuteAction(ExecuteActionPayload payload)
    {
        var request = new ExecuteActionRequest()
        {
            User = GetClientFromConnection(), // Can access helper from main file
            ActionId = payload.ActionId
        };
        
        var result = await _mediator.Send(request);
        await SendErrorIfNotSuccessful(result);
    }
    
    public async Task ResolveInput(ExecuteInputPayload payload)
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