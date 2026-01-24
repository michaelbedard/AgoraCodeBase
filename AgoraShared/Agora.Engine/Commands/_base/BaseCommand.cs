using Agora.Core.Dtos.Game.Commands;

namespace Agora.Engine.Commands._base;

public abstract class BaseCommand(Player player)
{
    public int Id { get; set; }
    public Player Player { get; set;  } = player;

    protected abstract CommandDto CreateDto();
    
    public CommandDto ToDto()
    {
        var dto = CreateDto();
        
        dto.Id = Id;
        dto.PlayerId = Player.Id;
        
        return dto;
    }
}