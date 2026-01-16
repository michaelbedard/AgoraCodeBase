using Agora.Core.Contracts;
using Agora.Core.Contracts;
using Agora.Core.Contracts.Client;
using Domain.Entities.Runtime;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using Presentation.Hub;

namespace Presentation.Proxies;

public partial class HubProxy : IGameProxy, ILobbyProxy
{
    private readonly IConnectionService _connectionService;
    private readonly IHubContext<HubController, IClientContract> _hubContext;

    public HubProxy(
        IConnectionService connectionService, 
        IHubContext<HubController, IClientContract> hubContext)
    {
        _connectionService = connectionService;
        _hubContext = hubContext;
    }

    // =========================================================
    // GROUP MANAGEMENT
    // =========================================================

    public async Task AddUserToGroupAsync(RuntimeUser user, string groupId)
    {
        if (string.IsNullOrEmpty(user.ConnectionId))
        {
            Console.WriteLine($"[Warning] Cannot add {user.Username} to group {groupId}: No ConnectionId");
            return;
        }

        Console.WriteLine($"Adding user {user.Username} to group {groupId}");
        
        await _hubContext.Groups.AddToGroupAsync(user.ConnectionId, groupId);
        _connectionService.AddUserToGroup(groupId, user);
    }

    public async Task RemoveUserFromGroupAsync(RuntimeUser user, string groupId)
    {
        if (string.IsNullOrEmpty(user.ConnectionId)) return;

        Console.WriteLine($"Removing user {user.Username} from group {groupId}");

        await _hubContext.Groups.RemoveFromGroupAsync(user.ConnectionId, groupId);
        _connectionService.RemoveUserFromGroup(groupId, user);
    }

    public async Task SendError(string connectionId, string errorMessage)
    {
        Console.WriteLine($"[!] Error to {connectionId}: {errorMessage}");
        await _hubContext.Clients.Client(connectionId).HandleError(errorMessage);
    }

    // =========================================================
    // HELPERS (Accessible by other partial files)
    // =========================================================

    private void LogCall(string methodName, string targetUsername, object? data)
    {
        Console.WriteLine($"-> {methodName} to {targetUsername}: {JsonConvert.SerializeObject(data)}");
    }
    
    private void LogGroupCall(string methodName, string groupId, object? data)
    {
        var groupMembers = _connectionService.GetUsersByGroup(groupId)
            .Select(c => c.Username)
            .ToList();
        
        Console.WriteLine($"-> {methodName} to Group [{groupId}] ({groupMembers.Count} users): {JsonConvert.SerializeObject(data)}");
    }
}