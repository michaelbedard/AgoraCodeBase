using Agora.Engine.Commands._base;

namespace Agora.Engine;

public abstract partial class BaseGame
{
    // ------------------------------------------------------------
    // The Builder Pattern (Methods used by Child Games)
    // ------------------------------------------------------------

    protected void Allow(IGameAction action)
    {
        action.Id = ActionIdCount++;
        _allowedActionsByPlayer[action.Player].Add(action);
    }
    
    // Internal execute that handles animations
    protected void Execute(IGameAction action)
    {
        var actionResult = action.Execute();

        foreach (var player in Players)
        {
            var newAnims = actionResult.GetAnimationsFor(player);
            _animationsByPlayer[player] = _animationsByPlayer[player].Concat(newAnims).ToList();
        }
    }

    protected async Task<TResult> Ask<TPayload, TResult>(BaseGameInput<TPayload, TResult> inputAction)
    {
        var player = inputAction.Player;

        // Register the input
        inputAction.Id = ActionIdCount++;
        _inputActionByPlayers[player] = inputAction;

        // Create independent waiter
        var inputTcs = new TaskCompletionSource<bool>();
        _inputWaiters[player] = inputTcs;

        try 
        {
            // Pause execution here
            await inputTcs.Task;
        }
        finally
        {
            // Cleanup state
            _inputActionByPlayers[player] = null;
            _inputWaiters.Remove(player);
        }

        // Return Data
        if (inputAction.Result == null)
            throw new InvalidOperationException("Input waiter finished, but Result was null.");

        return inputAction.Result;
    }

    protected async Task<BaseGameAction> WaitForNextActionAsync()
    {
        _actionWaiter = new TaskCompletionSource<BaseGameAction>();
        return await _actionWaiter.Task;
    }
    
    protected void SetWhoIsTakingTurn(Player player)
    {
        _playersTakingTurn.Clear();
        _playersTakingTurn.Add(player.Id);
    }

    protected void SetWhoIsTakingTurn(List<Player> players)
    {
        _playersTakingTurn.Clear();
        _playersTakingTurn.AddRange(players.Select(p => p.Id));
    }
}