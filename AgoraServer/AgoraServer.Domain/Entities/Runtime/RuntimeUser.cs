using Agora.Core.Enums;

namespace Domain.Entities.Runtime;

public class RuntimeUser
{
    //profile
    public Guid Id { get; set; }
    public string Username { get; set; }
    public int Avatar { get; set; }
    public int Pronouns { get; set; }
    public Language[] Languages { get; set; }
    public bool IsBot { get; set; }
    public string ConnectionId { get; set; }
    
    // duo
    public RuntimeUser? Duo { get; set; }

    // game
    public bool IsInSearch { get; set; } 
    public Lobby? Lobby { get; set; }
    
    public List<RuntimeUser> PreviousPlayers { get; set; }
    
    
    public RuntimeUser()
    {
        Id = Guid.Empty;
        Username = string.Empty;
        Avatar = 0;
        Pronouns = 0;
        Duo = null;
        Lobby = null;
        ConnectionId = string.Empty;
        PreviousPlayers = new List<RuntimeUser>();
    }
}