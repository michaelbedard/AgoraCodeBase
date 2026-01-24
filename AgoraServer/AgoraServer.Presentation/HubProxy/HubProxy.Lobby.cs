using Agora.Core.Dtos;
using Agora.Core.Enums;

namespace Presentation.HubProxy;

public partial class HubProxy
{
    public async Task BroadcastUserJoinedLobby(string groupId, UserDto userDto)
    {
        LogGroupCall(nameof(BroadcastUserJoinedLobby), groupId, userDto);
        await _hubContext.Clients.Group(groupId).UserJoinedLobby(userDto);
    }
    
    public async Task BroadcastUserLeaveLobby(string groupId, string userId)
    {
        LogGroupCall(nameof(BroadcastUserLeaveLobby), groupId, userId);
        await _hubContext.Clients.Group(groupId).UserLeavedLobby(userId);
    }
    
    public async Task BroadcastGameSelection(string groupId, GameKey gameKey)
    {
        LogGroupCall(nameof(BroadcastGameSelection), groupId, gameKey);
        await _hubContext.Clients.Group(groupId).GameSelected(gameKey);
    }
}