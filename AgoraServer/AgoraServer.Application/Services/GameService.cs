using Agora.Core.Actors;
using Agora.Core.Payloads.Hubs;
using Agora.Engine;
using Domain.Entities.Runtime;
using Domain.Interfaces.Proxies;
using Domain.Interfaces.Services;
using Domain.Mappers;

namespace Application.Services;

public class GameService : IGameService
{
    private readonly IGameProxy _gameProxy;
    private readonly ISessionService _sessionService;

    public GameService(IGameProxy gameProxy, ISessionService sessionService)
    {
        _gameProxy = gameProxy;
        _sessionService = sessionService;
    }

    public async Task<Result> LoadGameAsync(Lobby lobby)
    {
        // 1. Initialize Logic
        lobby.GameEngine = new GameEngine();
        
        var loadResult = lobby.GameEngine.LoadGame(lobby.GameId, lobby.Players.Select(p => p.ToUserDto()).ToList());
        if (!loadResult.IsSuccess) return Result.Failure(loadResult.Error);

        var loadDto = loadResult.Value;
        
        // 2. Update Lobby State
        lobby.GameIsRunning = true;

        // 3. Broadcast
        await _gameProxy.BroadcastLoadGameAsync(lobby.Id, loadDto);
        
        return Result.Success();
    }

    public async Task<Result> StartGameAsync(Lobby lobby)
    {
        // 1. Hook up Events
        lobby.GameEngine.OnUpdateGame += async (updates) => await HandleGameUpdate(lobby, updates);
        lobby.GameEngine.OnEndGame += async (endPayload) => await HandleGameEnd(lobby, endPayload);
        lobby.GameEngine.OnError += async (msg) => await StopGameAsync(lobby, msg);

        // 2. Broadcast Start
        await _gameProxy.BroadcastStartGameAsync(lobby.Id);

        // 3. Start Logic
        lobby.GameEngine.StartGame();

        return Result.Success();
    }

    public async Task<Result> StopGameAsync(Lobby lobby, string message)
    {
        Console.WriteLine($"[GameService] Stopping game in lobby {lobby.Id}: {message}");
        
        lobby.GameIsRunning = false;
        await _gameProxy.SendError(lobby.Id, message);

        // Cleanup Bots
        foreach (var player in lobby.Players.Where(p => p.IsBot).ToList())
        {
            _sessionService.RemoveSession(player.Id);
        }

        return Result.Success();
    }

    // --- Private Helpers ---

    private async Task HandleGameUpdate(Lobby lobby, Dictionary<string, UpdateGamePayload> updates)
    {
        foreach (var (username, payload) in updates)
        {
            // Use _sessionManager here
            var userResult = _sessionService.GetSessionByUsername(username);
            
            if (!userResult.IsSuccess) continue; // User might have disconnected

            var user = userResult.Value;

            if (user.IsBot)
            {
                // Fire and forget bot logic
                _ = RunBotAI(lobby.GameEngine, user, payload);
            }
            else
            {
                await _gameProxy.UpdateGameAsync(user, payload);
            }
        }
    }

    private async Task HandleGameEnd(Lobby lobby, EndGamePayload payload)
    {
        lobby.GameIsRunning = false;
        await _gameProxy.BroadcastEndGameAsync(lobby.Id, payload);
    }

    private async Task RunBotAI(GameEngine logic, RuntimeUser bot, UpdateGamePayload payload)
    {
        try
        {
            // Simulate "thinking" time
            await Task.Delay(Random.Shared.Next(2000, 3000));

            if (payload.Inputs.Any())
            {
                var input = payload.Inputs.OrderBy(x => Random.Shared.Next()).First();
                if (logic.CanPerformInput(bot.Username, input.Id))
                {
                    logic.PerformInput(bot.Username, input.Id, 0); 
                }
            }
            else if (payload.Actions.Any())
            {
                var action = payload.Actions.OrderBy(x => Random.Shared.Next()).First();
                if (logic.CanPerformAction(bot.Username, action.Id))
                {
                    logic.PerformAction(bot.Username, action.Id);
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Bot Error] {bot.Username}: {ex.Message}");
        }
    }
}