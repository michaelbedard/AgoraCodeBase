using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class DragStartSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }
        public Vector2 Position { get; set; }

        public DragStartSignal(InputObject inputObject, Vector2 position)
        {
            InputObject = inputObject;
            Position = position;
        }
    }
}