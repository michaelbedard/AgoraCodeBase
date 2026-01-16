using System.Collections.Generic;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Game.Modules.Card
{
    public partial class Card
    {
        private bool _showingBackOnDragStart;
            
        // on drag start
        protected override void OnDragStart(Vector2 position)
        {
            base.OnDragStart(position);

            _showingBackOnDragStart = Model.ShowingBack;
        }
        
        // on drag update
        protected override void OnDragUpdate(InputObject inputObject, Vector2 position)
        {
            var tangentialRotation = _showingBackOnDragStart ? 0f : 180f;
            _cameraPlaneService.PositionElementAtCursor(Transform, 0f, tangentialRotation, offset: 0.01f);
        }
        
        // on drag end
        protected override bool OnDragEnd(List<InputObject> colliders)
        {
            var hasMadeAction = base.OnDragEnd(colliders);
            
            if (hasMadeAction)
            {
                Model.Hand.CardBeingDrag = null;
                return false;
            }
        
            // check if add back to hand
            if (Slot.HasValue)
            {
                Model.Hand.AddCard(this, Slot.Value);
                Model.Hand.UpdateCardPositions();
            }

            Model.Hand.CardBeingDrag = null;
            return false;
        }
    }
}