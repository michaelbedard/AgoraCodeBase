using Agora.Core.Actors;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Services;

public interface ILobbyService
{
    // Room Management
    Result RegisterLobby(Lobby lobby);
    Result RemoveLobby(string lobbyId); // Good to have for cleanup
    Result<Lobby> GetLobby(string lobbyId);
    bool LobbyExist(string lobbyId);

    // Player Movement (Async because it calls the Proxy)
    Task<Result<Lobby>> JoinPlayerAsync(string lobbyId, RuntimeUser user);
    Task<Result> LeavePlayerAsync(string lobbyId, RuntimeUser user);
}