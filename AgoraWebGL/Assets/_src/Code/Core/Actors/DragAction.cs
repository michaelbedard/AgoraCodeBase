using Agora.Core.Dtos.Game.Commands;

namespace _src.Code.Core.Actors
{
    public class DragAction : GameAction
    {
        public int Id { get; set; }
        public string[] TargetIds { get; set; }
        public bool CanDropAnywhere { get; set; }

        
        public DragAction(CommandDto commandDto)
        {
            Id = commandDto.Id;
            TargetIds = new string[] {};
            CanDropAnywhere = true;
        }
        
        public DragAction(CommandDto commandDto, string[] targetIds)
        {
            Id = commandDto.Id;
            TargetIds = targetIds;
            CanDropAnywhere = false;
        }
    }
}