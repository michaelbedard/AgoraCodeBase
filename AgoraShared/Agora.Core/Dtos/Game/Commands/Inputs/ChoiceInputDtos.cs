namespace Agora.Core.Dtos.Game.Commands.Inputs;

public class ChoiceInputDto : CommandDto
{
    public string Label { get; set; }
    public ChoiceDto[] Choices { get; set; }
}

public class ChoiceDto
{
}

public class GameModuleChoiceDto : ChoiceDto
{
    public string GameModuleId { get; set; }
}

public class TextChoiceDto : ChoiceDto
{
    public string Text { get; set; }
    public string CardBackgroundAsset { get; set; }
}