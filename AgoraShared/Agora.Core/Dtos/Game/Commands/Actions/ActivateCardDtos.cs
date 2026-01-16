namespace Agora.Core.Dtos.Game.Commands.Actions;

public class ActivateCardActionDto : CommandDto
{
    public string CardId { get; set; }
}

public class ActivateCardAnimationDto : ActivateCardActionDto
{
    public bool ShouldAnimate { get; set; }
}