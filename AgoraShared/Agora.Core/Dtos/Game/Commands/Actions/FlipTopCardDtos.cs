namespace Agora.Core.Dtos.Game.Commands.Actions;

public class FlipTopCardActionDto : CommandDto
{
    public string DeckId { get; set; }
    public string ZoneId { get; set; }
}

public class FlipTopCardAnimationDto : FlipTopCardActionDto
{
    public string CardId { get; set; }
}