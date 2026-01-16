namespace Agora.Core.Dtos.Game.Commands.Other;

public class PlayerTurnCommandDto : CommandDto
{
    public List<string> PlayersTakingTurn { get; set; }
}