namespace Agora.Core.Dtos.Lobby;

public class LobbyDto
{
    public string Id { get; set; }
    public bool GameIsRunning { get; set; }
    public List<UserDto> Players { get; set; } = new();
}