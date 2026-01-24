using Agora.Core.Dtos;
using Agora.Core.Enums;

namespace Agora.Core.Contracts.Client;

public interface ILobbyClientContract
{
    // lobby
    Task UserJoinedLobby(UserDto userDto);
    Task UserLeavedLobby(string userId);
    Task GameSelected(GameKey gameKey);
}