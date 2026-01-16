using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Payloads.Hubs;

namespace Agora.Engine;

public class GameEngine
{
    /// <summary>
    /// Events
    /// </summary>
    public event Action<Dictionary<string, UpdateGamePayload>> OnUpdateGame;
    public event Action<EndGamePayload> OnEndGame;
    public event Action<string> OnError;

    /// <summary>
    /// LoadGame
    /// </summary>
    public Result<LoadGamePayload> LoadGame(string gameId, List<UserDto> players, bool verboseMode = false)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// StartGame
    /// </summary>
    public void StartGame()
    {
        
    }

    /// <summary>
    /// CanPerformAction
    /// </summary>
    public bool CanPerformAction(string playerUsername, int actionCommandId)
    {
        return false;
    }

    /// <summary>
    /// PerformAction
    /// </summary>
    public void PerformAction(string playerUsername, int actionCommandId)
    {
        
    }

    /// <summary>
    /// CanPerformInput
    /// </summary>
    public bool CanPerformInput(string playerUsername, int inputCommandId)
    {
        return false;
    }

    /// <summary>
    /// PerformInput
    /// </summary>
    public void PerformInput(string playerUsername, int inputCommandId, object answer)
    {
        
    }
    
}