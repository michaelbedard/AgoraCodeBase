namespace Agora.Engine.Commands._base;

public interface IGameAction : IGameCommand
{
    public GameActionResult Execute();
}