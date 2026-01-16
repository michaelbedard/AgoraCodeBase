using Agora.Core.Dtos.Game.Commands;

namespace _src.Code.Core.Signals.Game
{
    public class GameAnimationSignal : BaseSignal
    {
        public readonly CommandDto Animation;

        public GameAnimationSignal(CommandDto animation)
        {
            Animation = animation;
        }
    }
}