using Agora.Core.Actors;
using Agora.Core.Dtos;
using Agora.Core.Enums;
using Agora.Core.Payloads.Hubs;
using Agora.Engine.Games;
using Agora.Engine.Games.Uno;

namespace Agora.Engine;

public class GameEngine
{
    private IGame _activeGame;
    private bool _isGameRunning;
    
    /// <summary>
    /// Events
    /// </summary>
    public event Action<Dictionary<string, UpdateGamePayload>> OnUpdateGame;
    public event Action<EndGamePayload> OnEndGame;
    public event Action<string> OnError;

    /// <summary>
    /// LoadGame
    /// </summary>
    public Result<LoadGamePayload> LoadGame(GameKey gameKey, List<UserDto> players, bool verboseMode = false)
    {
        // 1. Factory Logic: Switch on the Game ID to load rules
        switch (gameKey)
        {
            case GameKey.Uno:
                _activeGame = new Uno();
                break;
            case GameKey.Coup:
                // _activeGame = new ChessGameLogic();
                break;
            case GameKey.LoveLetter:
                // _activeGame = new ChessGameLogic();
                break;
            default:
                return Result<LoadGamePayload>.Failure("Game Key not found.");
        }

        // 2. Initialize the game state
        var loadGamePayload = _activeGame.LoadGame(players);
        
        return Result<LoadGamePayload>.Success(loadGamePayload);
    }

    /// <summary>
    /// StartGame
    /// </summary>
    public void StartGame()
    {
        if (_activeGame == null)
        {
            OnError?.Invoke("No game loaded.");
            return;
        }

        _isGameRunning = true;
        _activeGame.StartGame();
        BroadcastGameState();
    }

    public void ExecuteAction(string playerUsername, int actionCommandId)
    {
        if (!_activeGame.CanPerformAction(playerUsername, actionCommandId)) 
        {
            OnError?.Invoke("Invalid Action Attempted");
            return;
        }

        _activeGame.ExecuteAction(playerUsername, actionCommandId);
        BroadcastGameState();
    }

    public void ExecuteInput(string playerUsername, int inputCommandId, object answer)
    {
        if (!_activeGame.CanExecuteInput(playerUsername, inputCommandId))
        {
            OnError?.Invoke("Invalid Input Attempted");
            return;
        }
        
        _activeGame.ExecuteInput(playerUsername, inputCommandId, answer);
        BroadcastGameState();
    }
    
    /// <summary>
    /// Helper to get the state from the logic and fire the event
    /// </summary>
    private void BroadcastGameState()
    {
        if (_activeGame == null) return;

        var playerStates = _activeGame.GetUpdateGamePayload();
        OnUpdateGame?.Invoke(playerStates);
    }
}