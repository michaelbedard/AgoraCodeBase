using Agora.Core.Payloads.Hubs;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Proxies;

public interface IGameProxy : IBaseProxy
{
    // game
    Task BroadcastLoadGameAsync(string groupId, LoadGamePayload payload);
    Task UpdateGameAsync(RuntimeUser user, UpdateGamePayload updateGamePayload);
    Task BroadcastEndGameAsync(string groupId, EndGamePayload payload);
}