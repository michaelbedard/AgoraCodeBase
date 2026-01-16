namespace Agora.Core.Dtos.Game.Commands.Actions;

public class ReturnCardToDeckActionDto : CommandDto
{
    public string CardId { get; set; }
    public string DeckId { get; set; }
}

public class ReturnCardToDeckAnimationDto : ReturnCardToDeckActionDto
{
}