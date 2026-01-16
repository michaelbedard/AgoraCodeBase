using System.Collections.Generic;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Signals.Inputs
{
    public class DragEndSignal : BaseSignal
    {
        public InputObject InputObject { get; set; }
        public List<InputObject> DroppedOnInputObjects { get; set; }
        public Vector2 Position { get; set; }

        public DragEndSignal(InputObject inputObject, List<InputObject> droppedOnInputObjects, Vector2 position)
        {
            InputObject = inputObject;
            DroppedOnInputObjects = droppedOnInputObjects;
            Position = position;
        }
    }
}