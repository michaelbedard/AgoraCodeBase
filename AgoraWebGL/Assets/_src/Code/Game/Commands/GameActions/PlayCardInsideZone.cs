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
    public class PlayCardInsideZone : BaseAction<PlayCardInsideZoneActionDto, PlayCardInsideZoneAnimationDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IParticleSystemService _particleSystemService;
        
        private const float CardTransitionTime = 0.5f;
        
        [Inject]
        public PlayCardInsideZone(
            SignalBus signalBus,
            IAnimationQueueService animationQueueService, 
            IGameModuleService gameModuleService, 
            IGameDataService gameDataService,
            IParticleSystemService particleSystemService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
            _gameDataService = gameDataService;
            _particleSystemService = particleSystemService;
        }

        protected override void AllowCore(PlayCardInsideZoneActionDto action)
        {
            var card = GameModuleService.GetGameModuleById(action.CardId);
            var zone = GameModuleService.GetGameModuleById(action.ZoneId);
            card.DragActions.Add(new DragAction(action, new [] {zone.Id}));

            if (action.CanDropAnywhere)
            {
                card.DragActions.Add(new DragAction(action)); // allow to drop anywhere
            }
        }

        protected override void AnimateCore(PlayCardInsideZoneAnimationDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
            //     var player = GameModuleService.GetGameModuleById<IPlayer>(animation.PlayerId);
            //     var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);
            //     var zone = GameModuleService.GetGameModuleById<IZone>(animation.ZoneId);
            //     
            //     var hand = player.Hand;
            //     var cardTransform = card.Transform;
            //     
            //     // add card
            //     var targetTransform = zone.AddCard(card);
            //     
            //     // remove card
            //     if (hand.Contains(card))
            //     {
            //         hand.RemoveCard(card);
            //         card.HidePreview();
            //     }
            //     
            //     // set properties
            //     card.CanBeDrag = false;
            //     
            //     // make copy of the card for later
            //     var cardCopyTransform = GameObject.Instantiate(cardTransform, null);
            //
            //     cardCopyTransform.transform.position = card.Transform.position;
            //     cardCopyTransform.transform.rotation = card.Transform.rotation;
            //     cardCopyTransform.transform.localScale = card.Transform.localScale;
            //     
            //     // sound
            //     AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipPlayingCard);
            //     
            //     // move to the deck
            //     card.Transform.SetParent(zone.Transform);
            //
            //     // set layer
            //     foreach (var cardInsideZone in zone.Cards)
            //     {
            //         cardInsideZone.SetLayer(cardInsideZone == card ? "ZoneTopCard" : "CardsInZone");
            //     }
            //     
            //     var s = DOTween.Sequence();
            //     s.Join(cardTransform.DOMove(targetTransform.position, CardTransitionTime));
            //     s.Join(cardTransform.DORotateQuaternion(targetTransform.rotation, CardTransitionTime));
            //     s.Join(cardTransform.DOScale(targetTransform.localScale, CardTransitionTime));
            //     s.Join(player.Hand.UpdateCardPositions(CardTransitionTime));
            //     
            //     
            //     // animate
            //     if (player.Id == _gameDataService.PlayerId)
            //     {
            //         // it is us
            //
            //         // make it go transparent
            //         s.JoinCallback(() =>
            //         {
            //             var canvasGroup = cardCopyTransform.gameObject.GetComponentInChildren<CanvasGroup>();
            //
            //             // TODO FADE
            //             
            //             canvasGroup.alpha = 0f;
            //         });
            //
            //         s.JoinCallback(() => 
            //         {
            //             // particle animation
            //             _particleSystemService.Play(cardCopyTransform);
            //         });
            //     }
            //     else
            //     {
            //         // // it is an opponent
            //         //
            //         // var targetCopyTransform = hand.GetTransformInFront(card, 1f, true, false);
            //         //
            //         // // move card to position in front of the hand
            //         // s.Join(cardCopyTransform.DOLocalMove(targetCopyTransform.localPosition, CardTransitionTime));
            //         // s.Join(cardCopyTransform.DORotateQuaternion(Quaternion.Euler(targetCopyTransform.eulerAngles), CardTransitionTime));
            //         // s.Join(cardCopyTransform.DOScale(targetCopyTransform.localScale, CardTransitionTime));
            //         //
            //         // s.Join(player.Hand.UpdateCardPositions(CardTransitionTime));
            //         //
            //         // // wait
            //         // s.AppendInterval(2f);
            //         //
            //         // // TODO make it go transparent
            //         //
            //         // s.AppendCallback(() =>
            //         // {
            //         //     GameObject.Destroy(targetTransform.gameObject);
            //         // });
            //     }
            //
            //     s.OnComplete(() =>
            //     {
            //         GameObject.Destroy(cardCopyTransform.gameObject);
            //         
            //         AnimationQueueService.Next();
            //     });
            });
        }
    }
}