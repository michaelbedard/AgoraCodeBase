using Domain.Entities;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Repositories;

public interface ILobbyRepository
{
    bool AddLobby(Lobby lobby);
    Lobby GetLobbyById(string lobbyId);
    bool LobbyExists(string lobbyId);
    bool RemoveLobby(string lobbyId);
    IEnumerable<Lobby> GetAllLobbies();
}