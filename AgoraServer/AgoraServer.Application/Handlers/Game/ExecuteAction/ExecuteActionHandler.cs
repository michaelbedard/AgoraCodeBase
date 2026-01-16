using Agora.Core.Actors;
using Domain.Interfaces.Services;

namespace Application.Handlers.Game.ExecuteAction;

public class ExecuteActionHandler : BaseHandler<ExecuteActionRequest, Result>
{
    private readonly IGameService _gameService;

    public ExecuteActionHandler(IGameService gameService)
    {
        _gameService = gameService;
    }
    
    public override async Task<Result> Handle(ExecuteActionRequest request, CancellationToken cancellationToken)
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
        gameEngine.PerformAction(user.Username, request.ActionId);

        return Result.Success();
    }
}