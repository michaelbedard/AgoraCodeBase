using Agora.Core.Dtos.Game.Commands;

namespace Agora.Engine.Commands._base;

public class GameActionResult
{
    // 1. Direct messages: "Send X to Player A"
    private readonly Dictionary<Player, List<CommandDto>> _specific = new();
    
    // 2. Broadcasts: "Send Y to Everyone"
    private readonly List<CommandDto> _global = new();
    
    // 3. Conditional: "Send Z to Everyone EXCEPT Player A"
    private readonly Dictionary<Player, List<CommandDto>> _globalExcept = new();

    // --- Builders ---

    public void SendTo(Player player, CommandDto anim)
    {
        if (!_specific.ContainsKey(player)) _specific[player] = new();
        _specific[player].Add(anim);
    }
    
    public void SendToAll(CommandDto anim)
    {
        _global.Add(anim);
    }
    
    public void SendToExcept(Player excludedPlayer, CommandDto anim)
    {
        // "This animation goes to everyone, but if you are 'excludedPlayer', skip it."
        if (!_globalExcept.ContainsKey(excludedPlayer)) _globalExcept[excludedPlayer] = new();
        _globalExcept[excludedPlayer].Add(anim);
    }

    // --- Resolver (Called by BaseGame) ---
    
    public List<CommandDto> GetAnimationsFor(Player player)
    {
        var result = new List<CommandDto>();

        // 1. Add Global Animations
        result.AddRange(_global);

        // 2. Add "Except" Animations (Only if this player is NOT the excluded one)
        foreach (var kvp in _globalExcept)
        {
            var excludedPlayer = kvp.Key;
            var anims = kvp.Value;
            
            if (player != excludedPlayer)
            {
                result.AddRange(anims);
            }
        }

        // 3. Add Specific Animations
        if (_specific.TryGetValue(player, out var specificAnims))
        {
            result.AddRange(specificAnims);
        }

        return result;
    }
}