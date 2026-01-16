using Agora.Core.Actors;

namespace Application.Handlers.Game.ExecuteAction;

public class ExecuteActionRequest : BaseRequest<Result>
{
    public int ActionId { get; set; }
}