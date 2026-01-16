using Agora.Core.Actors;
using Domain.Entities.Runtime;

namespace Domain.Interfaces.Services;

public interface IGameService
{
    Task<Result> LoadGameAsync(Lobby lobby);
    Task<Result> StartGameAsync(Lobby lobby);
    Task<Result> StopGameAsync(Lobby lobby, string message);
}