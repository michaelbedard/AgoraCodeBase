namespace Agora.Core.Dtos.Game.Commands.Actions;

public class ShuffleDeckActionDto : CommandDto
{
    public string DeckId { get; set; }
}

public class ShuffleDeckAnimationDto : ShuffleDeckActionDto
{
}