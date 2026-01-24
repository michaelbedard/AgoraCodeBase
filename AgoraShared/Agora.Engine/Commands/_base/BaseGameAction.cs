namespace Agora.Engine.Commands._base;

public abstract class BaseGameAction : BaseCommand, IGameAction
{
    public abstract GameActionResult Execute();

    protected BaseGameAction(Player player) : base(player)
    {
        Player = player;
    }
}