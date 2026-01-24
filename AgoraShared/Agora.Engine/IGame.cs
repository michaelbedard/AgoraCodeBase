using Agora.Core.Dtos;
using Agora.Core.Payloads.Hubs;
using Agora.Engine.Games;

namespace Agora.Engine;

public interface IGame
{
    LoadGamePayload LoadGame(List<UserDto> players);
    void StartGame();
    Dictionary<string, UpdateGamePayload> GetUpdateGamePayload();
    
    // Action handling
    bool CanPerformAction(string playerId, int actionCommandId);
    void ExecuteAction(string playerId,int actionCommandId);
    bool CanExecuteInput(string playerId,int inputCommandId);
    void ExecuteInput(string playerId,int inputCommandId, object payload);

}