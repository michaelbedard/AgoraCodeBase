namespace _src.Code.Core.Signals.Other
{
    public class ResponseSignal<TResponse>
    {
        public TResponse Response { get; }

        public ResponseSignal(TResponse response)
        {
            Response = response;
        }
    }
}