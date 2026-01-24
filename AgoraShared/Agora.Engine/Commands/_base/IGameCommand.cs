using Agora.Core.Dtos.Game.Commands;

namespace Agora.Engine.Commands._base;

public interface IGameCommand
{
    int Id { get; set; }
    Player Player { get; }
    public CommandDto ToDto();
}