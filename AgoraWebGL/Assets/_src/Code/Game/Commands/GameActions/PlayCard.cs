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
    public class PlayCard : BaseAction<PlayCardActionDto, PlayCardAnimationDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IParticleSystemService _particleSystemService;
        
        private const float CardTransitionTime = 0.5f;
        
        [Inject]
        public PlayCard(
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
        
        // allow
        protected override void AllowCore(PlayCardActionDto action)
        {
            var card = GameModuleService.GetGameModuleById(action.CardId);
            card.DragActions.Add(new DragAction(action));
        }
        
        // animate
        protected override void AnimateCore(PlayCardAnimationDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
            //     var player = GameModuleService.GetGameModuleById<IPlayer>(animation.PlayerId);
            //     var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);
            //
            //     var hand = player.Hand;
            //     var cardTransform = card.Transform;
            //     
            //     // remove card
            //     if (hand.Contains(card))
            //     {
            //         hand.RemoveCard(card);
            //     }
            //     
            //     // sound
            //     AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipPlayingCard);
            //
            //     // animate
            //     var s = DOTween.Sequence();
            //     if (player.Id == _gameDataService.PlayerId)
            //     {
            //         // it is us
            //
            //         // make it go transparent
            //          // s.Join(card.CanvasGroup.DOFade(0f, 0.5f));
            //
            //         s.JoinCallback(() => 
            //         {
            //             // particle animation
            //             _particleSystemService.Play(card.Transform);
            //         });
            //     }
            //     else
            //     {
            //         // it is an opponent
            //         var targetTransform = hand.GetTransformInFront(card, 1f, true, false);
            //         
            //         // move card to position in front of the hand
            //         s.Join(cardTransform.DOLocalMove(targetTransform.localPosition, CardTransitionTime));
            //         s.Join(cardTransform.DORotateQuaternion(Quaternion.Euler(targetTransform.eulerAngles), CardTransitionTime));
            //         s.Join(cardTransform.DOScale(targetTransform.localScale, CardTransitionTime));
            //         
            //         s.Join(player.Hand.UpdateCardPositions(CardTransitionTime));
            //         
            //         // wait
            //         s.AppendInterval(2f);
            //         
            //         // go transparent
            //         // s.Append(card.CanvasGroup.DOFade(0f, 0.5f));
            //     }
            //
            //     s.OnComplete(() =>
            //     {
            //         // reset
            //         card.Transform.position = Vector3.zero;
            //         card.Transform.localScale = Vector3.one;
            //         card.Transform.localRotation = new Quaternion();
            //         card.CanvasGroup.alpha = 1f;
            //         
            //         AnimationQueueService.Next();
            //     });
            });
        }
    }
}