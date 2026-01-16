namespace Agora.Core.Dtos.Game.Commands.Actions;

public class TransferTokenActionDto : CommandDto
{
    public string TokenId { get; set; }
    public string ZoneId { get; set; }
}

public class TransferTokenAnimationDto : TransferTokenActionDto
{
}