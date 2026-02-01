using _src.Code.Core.Signals.Inputs;

namespace _src.Code.Game._other.Hand
{
    public partial class Hand
    {
        private void HandleDragStart(DragStartSignal signal)
        {
            if (signal.InputObject == null || !signal.InputObject.CanBeDrag)
                return;
            
            var cardInHandBeingDrag = _cardsInHand.Find(c => c.InputObject == signal.InputObject);
            
            // return if not dragging a card from hand
            if (cardInHandBeingDrag == null) 
                return;
            
            CardBeingDrag = cardInHandBeingDrag;
            CardBeingDrag.Slot = _cardsInHand.IndexOf(CardBeingDrag);
            CardBeingDrag.SetLayer("AboveEverything");
            
            UpdateCardPositions();
        }
        
        private void HandleDragUpdate(DragUpdateSignal signal)
        {
            if (signal.InputObject == null || !signal.InputObject.CanBeDrag)
                return;
            
            if (CardBeingDrag == null) return;
            
            UpdateCardPositions();
        }
    }
}