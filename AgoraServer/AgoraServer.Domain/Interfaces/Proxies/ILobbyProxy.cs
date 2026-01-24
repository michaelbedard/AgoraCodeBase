using Agora.Core.Dtos;
using Agora.Core.Enums;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Proxies;

public interface ILobbyProxy : IBaseProxy
{
    Task BroadcastUserJoinedLobby(string groupId, UserDto userDto);
    Task BroadcastUserLeaveLobby(string groupId, string userId);
    Task BroadcastGameSelection(string groupId, GameKey gameKey);
}