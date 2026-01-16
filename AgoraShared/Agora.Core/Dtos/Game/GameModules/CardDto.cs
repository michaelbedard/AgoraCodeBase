namespace Agora.Core.Dtos.Game.GameModules;

public class CardDto : GameModuleDto
{
    public string StartZoneId { get; set; }
    public string FrontImage { get; set; }
    public string BackImage { get; set; }
    public string Text { get; set; }
}