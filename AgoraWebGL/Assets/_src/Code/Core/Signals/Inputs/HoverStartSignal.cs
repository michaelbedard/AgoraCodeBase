using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class HoverStartSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }

        public HoverStartSignal(InputObject inputObject)
        {
            InputObject = inputObject;
        }
    }
}