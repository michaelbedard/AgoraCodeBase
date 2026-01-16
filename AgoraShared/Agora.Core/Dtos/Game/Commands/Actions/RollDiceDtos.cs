namespace Agora.Core.Dtos.Game.Commands.Actions;

public class RollDiceActionDto : CommandDto
{
    public string DiceId { get; set; }
}

public class RollDiceAnimationDto : RollDiceActionDto
{
    public int RollResult { get; set; }
}