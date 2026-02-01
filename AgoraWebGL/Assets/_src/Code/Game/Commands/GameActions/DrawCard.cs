using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Actions;

using DG.Tweening;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Commands.GameActions
{
    public class DrawCard : BaseAction<DrawCardActionDto, DrawCardAnimationDto>
    {
        [Inject]
        private IClientDataService ClientDataService { get; set; }
        
        [Inject]
        public DrawCard(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
        }
        
        // allow
        protected override void AllowCore(DrawCardActionDto action)
        {
            var deck = GameModuleService.GetGameModuleById(action.DeckId);
            deck.ClickActions.Add(new ClickAction(action));
        }
        
        // animate
        protected override void AnimateCore(DrawCardAnimationDto animation)
        {
            AnimationQueueService.Push(() =>
            {
                var deck = GameModuleService.GetGameModuleById<IDeck>(animation.DeckId);
                var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);
                
                Debug.Log($"Animation for {card.Id}");

                var topDeckTransform = deck.GetTopDeckTransform();
                var cardTransform = card.Transform;

                // position card at the top of the deck instantaneously
                cardTransform.position = topDeckTransform.position;
                cardTransform.rotation = topDeckTransform.rotation;
                
                Debug.Log($"Card positionned");
            
                // add card to the hand and animate
                var hand = ClientDataService.PlayerIdToHand[animation.PlayerId];
                hand.AddCard(card, 0);
                
                Debug.Log($"Card added to hand");
                
                // sound
                AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipDrawingCard);
                
                hand.UpdateCardPositions().OnComplete(() =>
                {
                    Debug.Log($"completed");
                    AnimationQueueService.Next();
                });
            });
        }
    }
}