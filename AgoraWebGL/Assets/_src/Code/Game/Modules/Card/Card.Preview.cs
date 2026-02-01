using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using DG.Tweening;
using UnityEngine;

namespace _src.Code.Game.Modules.Card
{
    public partial class Card
    {
        private bool _isShowingPreview;
        private ICard _previewCard;
        
        private void ShowPreview()
        {
            if (CanBeHover && !_isShowingPreview)
            {
                _isShowingPreview = true;

                if (Model.Hand == null)
                {
                    // we are hovering a card on board
                    
                    // TODO
                    Debug.Log("Hovering card " + Model.Id);
                }
                else
                {
                    // we are hovering card in hand
                    
                    // check if it's our card
                    if (Model.Hand.PlayerId != ServiceLocator.GetService<IClientDataService>().Id)
                    {
                        return;
                    }
                    
                    // make card transparent
                    CanvasGroup.alpha = 0f;
                
                    // get preview card
                    _previewCard = (ICard)Clone();
                    _previewCard.DisableCollider();
                    _previewCard.DisableGlow();
                
                    // get target transform
                    var targetTransform = Model.Hand.GetTransformInFront(this, 0.6f, true, true);
                
                    // copy position, rotation and scale
                    var previewCardTransform = _previewCard.Transform;
                    previewCardTransform.position = Transform.position;
                    previewCardTransform.rotation = Transform.rotation;
                    previewCardTransform.localScale = Transform.localScale;
                
                    // copy its visual
                    _previewCard.FrontImage.sprite = FrontImage.sprite;
                    _previewCard.BackImage.sprite = BackImage.sprite;
                    _previewCard.ClickActions = ClickActions;
                    _previewCard.DragActions = DragActions;
                    
                    _previewCard.UpdateGlowColor();
                
                    // set layer to above all
                    _previewCard.SetLayer("AboveEverything");
                
                    // animate preview card to target transform
                    DOTween.Sequence()
                        .Append(previewCardTransform.DOLocalMove(targetTransform.position, 0.2f))
                        .Join(previewCardTransform.DORotateQuaternion(targetTransform.rotation, 0.2f))
                        .Join(previewCardTransform.DOScale(targetTransform.localScale, 0.2f))
                        .OnComplete(() =>
                        {
                            Object.Destroy(targetTransform.gameObject);
                        });
                }
            }
        }
        
        public void HidePreview()
        {
            if (!_isShowingPreview)
            {
                return;
            }
            
            _isShowingPreview = false;
            CanvasGroup.alpha = 1f;

            if (_previewCard != null)
            {
                _previewCard.Destroy();
            }
        }
    }
}