using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Actions;

using DG.Tweening;
using Zenject;

namespace _src.Code.Game.Commands.GameActions
{
    public class FlipTopCard : BaseAction<FlipTopCardActionDto, FlipTopCardAnimationDto>
    {
        private const float CardTransitionTime = 1f;
        
        [Inject]
        public FlipTopCard(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
        }
        
        // allow
        protected override void AllowCore(FlipTopCardActionDto action)
        {
            var deck = GameModuleService.GetGameModuleById(action.DeckId);
            deck.ClickActions.Add(new ClickAction(action));
        }
        
        // animate
        protected override void AnimateCore(FlipTopCardAnimationDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
                var deck = GameModuleService.GetGameModuleById<IDeck>(animation.DeckId);
                var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);
                var zone = GameModuleService.GetGameModuleById<IZone>(animation.ZoneId);
                
                // add to zone
                var targetTransform = zone.AddCard(card);

                // get transforms
                var topDeckTransform = deck.GetTopDeckTransform();
                var cardTransform = card.Transform;
                
                // position card at the top of the deck instantaneously
                card.SetLayer("AboveDefault");
                cardTransform.position = topDeckTransform.position;
                cardTransform.rotation = topDeckTransform.rotation;
                
                // sound
                AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipPlayingCard);
                
                foreach (var cardInsideZone in zone.Cards)
                {
                    cardInsideZone.SetLayer(cardInsideZone == card ? "ZoneTopCard" : "CardsInZone");
                }
                
                // tween
                var s = DOTween.Sequence();
                s.Join(zone.UpdateObjectPositions(card));
                s.Join(cardTransform.DOMove(targetTransform.position, CardTransitionTime));
                s.Join(cardTransform.DORotateQuaternion(targetTransform.rotation, CardTransitionTime));
                s.Join(cardTransform.DOScale(targetTransform.localScale, CardTransitionTime));
                
                s.OnComplete(() =>
                {
                    AnimationQueueService.Next();
                });
            });
        }
    }
}