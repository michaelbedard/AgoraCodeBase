using System.Collections.Concurrent;
using Agora.Core.Actors;
using Domain.Entities.Runtime;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;
using Domain.Mappers;

namespace Application.Services;

public class LobbyService : ILobbyService
{
    private readonly ConcurrentDictionary<string, Lobby> _lobbies = new();
    private readonly ILobbyProxy _lobbyProxy;

    public LobbyService(ILobbyProxy lobbyProxy)
    {
        _lobbyProxy = lobbyProxy;
    }

    public Result RegisterLobby(Lobby lobby)
    {
        if (_lobbies.TryAdd(lobby.Id, lobby))
        {
            return Result<string>.Success(lobby.Id);
        }
        return Result.Failure($"Lobby {lobby.Id} already registered");
    }

    public Result RemoveLobby(string lobbyId)
    {
        if (_lobbies.TryRemove(lobbyId, out _))
        {
            return Result.Success();
        }
        return Result.Failure($"Lobby {lobbyId} not found or could not be removed.");
    }

    public Result<Lobby> GetLobby(string lobbyId)
    {
        if (_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            return Result<Lobby>.Success(lobby);
        }
        return Result<Lobby>.Failure($"Lobby {lobbyId} not found");
    }

    public bool LobbyExist(string lobbyId)
    {
        return _lobbies.ContainsKey(lobbyId);
    }

    public async Task<Result<Lobby>> JoinPlayerAsync(string lobbyId, RuntimeUser user)
    {
        if (!_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            return Result<Lobby>.Failure($"Lobby {lobbyId} not found");
        }

        if (lobby.Players.Contains(user))
        {
            return Result<Lobby>.Failure($"Player {user.Username} is already in the lobby.");
        }

        // 1. Update State
        user.Lobby = lobby;
        lobby.Players.Add(user);

        // 2. Notify Network (Side Effect)
        if (!user.IsBot)
        {
            await _lobbyProxy.AddUserToGroupAsync(user, lobbyId);
            await _lobbyProxy.BroadcastUserJoinedLobby(lobbyId, user.ToUserDto());
        }

        return Result<Lobby>.Success(lobby);
    }

    public async Task<Result> LeavePlayerAsync(string lobbyId, RuntimeUser user)
    {
        if (!_lobbies.TryGetValue(lobbyId, out var lobby))
        {
            return Result.Failure($"Lobby {lobbyId} not found");
        }

        // 1. Update State
        user.Lobby = null;
        lobby.Players = new List<RuntimeUser>(lobby.Players.Where(p => p.Id != user.Id));

        // 2. Notify Network
        if (!user.IsBot)
        {
            await _lobbyProxy.RemoveUserFromGroupAsync(user, lobbyId);
            await _lobbyProxy.BroadcastUserLeaveLobby(lobbyId, user.Id.ToString());
        }

        return Result.Success();
    }
}