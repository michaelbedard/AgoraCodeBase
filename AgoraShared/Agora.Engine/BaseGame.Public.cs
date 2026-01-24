using Agora.Core.Payloads.Hubs;
using Agora.Engine.Commands._base;

namespace Agora.Engine;

public abstract partial class BaseGame
{
    // ------------------------------------------------------------
    // The Public Interface (Called by GameEngine)
    // ------------------------------------------------------------

    public bool CanPerformAction(string playerId, int actionId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return false;
        
        return _allowedActionsByPlayer.ContainsKey(player) && 
               _allowedActionsByPlayer[player].Any(a => a.Id == actionId);
    }

    public void ExecuteAction(string playerId, int actionCommandId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return;
        
        var action = _allowedActionsByPlayer[player].Find(a => a.Id == actionCommandId);
        if (action == null) return;

        ResetAllowedActions();

        Execute(action);
        
        _actionWaiter?.TrySetResult(action as BaseGameAction);
    }

    public bool CanExecuteInput(string playerId, int inputId)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return false;
        
        return _inputActionByPlayers.ContainsKey(player) && 
               _inputActionByPlayers[player]?.Id == inputId;
    }

    public void ExecuteInput(string playerId, int inputCommandId, object payload)
    {
        var player = Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null) return;
        
        if (!_inputActionByPlayers.TryGetValue(player, out var activeInput) || activeInput == null)
        {
            Console.WriteLine($"[Warning] Player {player.Username} sent input, but none was expected.");
            return;
        }

        if (activeInput.Id != inputCommandId)
        {
            Console.WriteLine($"[Warning] Input ID mismatch. Expected {activeInput.Id}, got {inputCommandId}");
            return;
        }

        try
        {
            activeInput.Execute(payload);

            if (_inputWaiters.TryGetValue(player, out var tcs))
            {
                tcs.TrySetResult(true);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Error] Failed to execute input: {ex.Message}");
        }
    }

    public Dictionary<string, UpdateGamePayload> GetUpdateGamePayload()
    {
        var result = new Dictionary<string, UpdateGamePayload>();
        foreach (var player in Players)
        {
            result[player.Id] = new UpdateGamePayload()
            {
                Actions = _allowedActionsByPlayer[player].Select(a => a.ToDto()).ToList(),
                Input = _inputActionByPlayers.GetValueOrDefault(player)?.ToDto(),
                Descriptions = GetDescriptions(player),
                Animations = _animationsByPlayer[player],
                PlayersTakingTurn = _playersTakingTurn,
            };
        }

        ResetAnimations();
        return result;
    }
}