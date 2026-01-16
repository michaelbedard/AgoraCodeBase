using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class DragUpdateSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }
        public InputObject DraggedOverInputObject { get; set; }
        public Vector2 Position { get; set; }

        public DragUpdateSignal(InputObject inputObject, InputObject draggedOverInputObject, Vector2 position)
        {
            InputObject = inputObject;
            DraggedOverInputObject = draggedOverInputObject;
            Position = position;
        }
    }
}