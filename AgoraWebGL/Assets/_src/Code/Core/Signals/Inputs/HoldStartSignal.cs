using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class HoldStartSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }

        public HoldStartSignal(InputObject inputObject)
        {
            InputObject = inputObject;
        }
    }
}