using Agora.Core.Dtos.Lobby;
using Domain.Entities.Runtime;

namespace Domain.Mappers;

public static class LobbyMapper
{
    public static LobbyDto ToLobbyDto(this Lobby lobby)
    {
        if (lobby == null) throw new ArgumentNullException(nameof(lobby));
        return new LobbyDto
        {
            Id = lobby.Id,
            GameKey = lobby.GameKey,
            GameIsRunning = lobby.GameIsRunning,
            Players = lobby.Players.Select(player => player.ToUserDto()).ToList()
        };
    }
}