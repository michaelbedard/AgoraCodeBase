using Agora.Core.Dtos.Game.Commands;

namespace _src.Code.Core.Actors
{
    public class ClickAction : GameAction
    {
        public int Id { get; set; }

        public ClickAction(CommandDto commandDto)
        {
            Id = commandDto.Id;
        }
    }
}