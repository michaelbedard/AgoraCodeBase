namespace Agora.Core.Dtos.Game.Commands.Other;

public class AllowCommandDto : CommandDto
{
    public int Id { get; set; } = -1;
    public bool IsAction { get; set; }
}