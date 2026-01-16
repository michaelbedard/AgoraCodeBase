using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class HoverEndSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }

        public HoverEndSignal(InputObject inputObject)
        {
            InputObject = inputObject;
        }
    }
}