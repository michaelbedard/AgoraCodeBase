using Agora.Core.Payloads.Hubs;
using Domain.Entities.Runtime;

namespace Presentation.Proxies;

public partial class HubProxy
{
    public async Task BroadcastLoadGameAsync(string groupId, LoadGamePayload payload)
    {
        LogGroupCall(nameof(BroadcastLoadGameAsync), groupId, payload);
        await _hubContext.Clients.Group(groupId).LoadGame(payload);
    }

    public async Task BroadcastStartGameAsync(string groupId)
    {
        LogGroupCall(nameof(BroadcastStartGameAsync), groupId, null);
        await _hubContext.Clients.Group(groupId).StartGame();
    }

    public async Task UpdateGameAsync(RuntimeUser user, UpdateGamePayload payload)
    {
        LogCall(nameof(UpdateGameAsync), user.Username, payload);
        await _hubContext.Clients.Client(user.ConnectionId).UpdateGame(payload);
    }
    
    public async Task BroadcastEndGameAsync(string groupId, EndGamePayload payload)
    {
        LogGroupCall(nameof(BroadcastEndGameAsync), groupId, payload);
        await _hubContext.Clients.Group(groupId).EndGame(payload);
    }
}