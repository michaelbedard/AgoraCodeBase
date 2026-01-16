namespace Agora.Core.Dtos.Game.Commands.Other;

public class EndCommandDto : CommandDto
{
    public string Message { get; set; }
    public Dictionary<string, string> PlayerIdToInfo { get; set; }
}

public class CompetitiveEndCommandDto : EndCommandDto
{
    public string[] OrderedPlayerUsernames { get; set; }
}

public class CooperativeEndCommandDto : EndCommandDto
{
    public string[] WinnersUsername { get; set; }
    public string[] LosersIUsername { get; set; }
}

public class SingleWinnerEndCommandDto : EndCommandDto
{
    public string WinnerUsername { get; set; }
    public string[] LosersIUsername { get; set; }
}