namespace Agora.Core.Dtos.Game.Commands.Actions;

public class FlipDeckTopCardActionDto : CommandDto
{
    public string DeckId { get; set; }
    public string ZoneId { get; set; }
}

public class FlipDeckTopCardAnimationDto : FlipDeckTopCardActionDto
{
    public string CardId { get; set; }
}