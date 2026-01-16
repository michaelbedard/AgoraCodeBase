using Agora.Core.Actors;

namespace Application.Handlers.Game.ExecuteInput;

public class ExecuteInputRequest : BaseRequest<Result>
{
    public int InputId { get; set; }
    public object Input { get; set; }
}