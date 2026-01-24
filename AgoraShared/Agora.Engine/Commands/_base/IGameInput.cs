namespace Agora.Engine.Commands._base;

public interface IGameInput : IGameCommand
{
    GameActionResult Execute(object payload);
}