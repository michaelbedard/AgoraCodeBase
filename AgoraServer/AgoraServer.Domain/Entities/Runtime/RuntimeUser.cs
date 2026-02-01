using Agora.Core.Enums;

namespace Domain.Entities.Runtime;

public class RuntimeUser
{
    //profile
    public string Id { get; set; }
    public string Username { get; set; }
    public int Avatar { get; set; }
    public int Pronouns { get; set; }
    public Language[] Languages { get; set; }
    public string ChannelId { get; set; }
    public string ConnectionId { get; set; }

    // game
    public bool IsBot { get; set; }
    public Lobby? Lobby { get; set; }
    
    
    public RuntimeUser(string id, string username)
    {
        Id = id;
        Username = username;
        Avatar = 0;
        Pronouns = 0;
        Lobby = null;
        ChannelId = string.Empty;
        ConnectionId = string.Empty;
    }
}