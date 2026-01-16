using Agora.Core.Dtos.Game.Commands;

namespace _src.Code.Core.Signals.Game
{
    public class GameInputSignal
    {
        public readonly CommandDto Input;

        public GameInputSignal(CommandDto input)
        {
            Input = input;
        }
    }
}