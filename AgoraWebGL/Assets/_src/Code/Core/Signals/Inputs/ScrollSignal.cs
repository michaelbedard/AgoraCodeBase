using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class ScrollSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }
        public Vector2 ScrollDelta { get; set; }

        public ScrollSignal(InputObject inputObject, Vector2 scrollDelta)
        {
            InputObject = inputObject;
            ScrollDelta = scrollDelta;
        }
    }
}