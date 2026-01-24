using Agora.Core.Dtos;
using Agora.Core.Dtos.Game.GameModules;

namespace Agora.Engine;

public class Player
{
    public string Id { get; set; }
    public string Username { get; set; }
    public List<CardDto> Hand { get; set; } = new List<CardDto>();

    public Player(UserDto user)
    {
        Id = user.Id;
        Username = user.Username;
    }
}