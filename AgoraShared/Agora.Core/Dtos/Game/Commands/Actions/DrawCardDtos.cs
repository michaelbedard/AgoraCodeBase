namespace Agora.Core.Dtos.Game.Commands.Actions;

public class DrawCardActionDto : CommandDto
{
    public string DeckId { get; set; }
}

public class DrawCardAnimationDto : DrawCardActionDto
{
    public string PlayerId { get; set; }
    public string CardId { get; set; }
}