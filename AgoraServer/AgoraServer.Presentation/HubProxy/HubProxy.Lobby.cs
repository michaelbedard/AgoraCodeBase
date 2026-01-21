using Agora.Core.Dtos;

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
}