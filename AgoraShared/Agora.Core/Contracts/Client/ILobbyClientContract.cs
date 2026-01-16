using Agora.Core.Dtos;

namespace Agora.Core.Contracts.Client;

public interface ILobbyClientContract
{
    // lobby
    Task UserJoinedLobby(UserDto userDto);
    Task UserLeavedLobby(string userId);
}