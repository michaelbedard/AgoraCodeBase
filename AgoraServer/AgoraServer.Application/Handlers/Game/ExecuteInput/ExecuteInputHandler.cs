using Agora.Core.Actors;
using Domain.Interfaces.Services;

namespace Application.Handlers.Game.ExecuteInput;

public class ExecuteInputHandler : BaseHandler<ExecuteInputRequest, Result>
{
    private readonly IGameService _gameService;

    public ExecuteInputHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    public override async Task<Result> Handle(ExecuteInputRequest request, CancellationToken cancellationToken)
    {
        var user = request.User;
        
        ///////////////////////////////////////////////////////////////////////////
        // Edge Cases
        ///////////////////////////////////////////////////////////////////////////

        if (user.Lobby == null || user.Lobby.GameEngine == null)
        {
            return Result.Failure("XXX");
        }
        
        ///////////////////////////////////////////////////////////////////////////
        // Logic
        ///////////////////////////////////////////////////////////////////////////
        
        var gameEngine = user.Lobby.GameEngine;
        gameEngine.ExecuteInput(user.Username, request.InputId, request.Input);

        return Result.Success();
    }
}