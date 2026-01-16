using Agora.Core.Payloads.Hubs;

namespace _src.Code.Core.Signals.Game
{
    public class EndGameSignal : BaseSignal
    {
        public EndGamePayload EndGamePayload { get; set; }

        public EndGameSignal(EndGamePayload endGameDto)
        {
            EndGamePayload = endGameDto;
        }
    }
}