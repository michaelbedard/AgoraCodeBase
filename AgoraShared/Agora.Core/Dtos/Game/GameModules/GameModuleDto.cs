using Agora.Core.Actors;
using Agora.Core.Enums;

namespace Agora.Core.Dtos.Game.GameModules;

public class GameModuleDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public GameModuleType Type { get; set; }
    public bool IsPlayerModule { get; set; }
    public int Seat { get; set; }
    public Position? Position { get; set; }
}