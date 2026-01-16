using Agora.Core.Payloads.Hubs;

namespace Agora.Core.Contracts.Client;

public interface IGameClientContract
{
    // game
    Task LoadGame(LoadGamePayload loadGamePayload);
    Task StartGame();
    Task UpdateGame(UpdateGamePayload updateGamePayload);
    Task EndGame(EndGamePayload endGamePayload);
}