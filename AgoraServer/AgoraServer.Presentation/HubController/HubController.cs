using Agora.Core.Actors;
using Domain.Entities.Runtime;
using Domain.Interfaces.Services;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using Agora.Core.Contracts;
using Agora.Core.Contracts.Client;
using Serilog.Core;

namespace Presentation.Hub; 

public partial class HubController : Hub<IClientContract>
{
    private readonly ISessionService _sessionService;
    private readonly IConnectionService _connectionService;
    private readonly IMediator _mediator;

    public HubController(
        ISessionService sessionService, 
        IConnectionService connectionService,
        IMediator mediator)
    {
        _sessionService = sessionService;
        _connectionService = connectionService;
        _mediator = mediator;
    }

    protected RuntimeUser GetClientFromConnection()
    {
        // HubType is removed. We assume a generic "Main" connection type or simplify the service.
        var sender = _connectionService.GetUserByConnectionId(Context.ConnectionId);
        if (sender == null)
        {
            throw new Exception($"Connection {Context.ConnectionId} is not linked to any runtime user");
        }
        return sender;
    }

    protected async Task SendErrorIfNotSuccessful(Result result)
    {
        if (!result.IsSuccess)
        {
            await Clients.Caller.HandleError(result.Error);
        }
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.GetHttpContext()?.Request.Query["userId"].FirstOrDefault();
        
        if (string.IsNullOrEmpty(userId))
        {
            await Clients.Caller.HandleError("userId is required to connect.");
            Context.Abort();
            return;
        }
        
        var userResult = _sessionService.GetSessionById(userId);
        if (!userResult.IsSuccess)
        {
            await Clients.Caller.HandleError(userResult.Error);
            Context.Abort();
            return;
        }
        
        var user = userResult.Value;
        
        user.ConnectionId = Context.ConnectionId; 
        _connectionService.RegisterConnection(Context.ConnectionId, user);
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionService.UnregisterConnection(Context.ConnectionId);

        await base.OnDisconnectedAsync(exception);
    }
    
    // test method
    public async Task TestHub(string content)
    {
        Console.WriteLine(content);
    }
}