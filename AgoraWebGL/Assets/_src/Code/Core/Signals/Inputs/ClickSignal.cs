using _src.Code.Core.Actors;

namespace _src.Code.Core.Signals.Inputs
{
    public class ClickSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }

        public ClickSignal(InputObject inputObject)
        {
            InputObject = inputObject;
        }
    }
}