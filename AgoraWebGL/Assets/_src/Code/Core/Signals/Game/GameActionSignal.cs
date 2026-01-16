using Agora.Core.Dtos.Game.Commands;

namespace _src.Code.Core.Signals.Game
{
    public class GameActionSignal : BaseSignal
    {
        public readonly CommandDto Action;

        public GameActionSignal(CommandDto action)
        {
            Action = action;
        }
    }
}