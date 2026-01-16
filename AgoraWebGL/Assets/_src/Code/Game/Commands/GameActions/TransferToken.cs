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
    public class TransferToken : BaseAction<TransferTokenActionDto, TransferTokenAnimationDto>
    {
        private const float MoveTransitionTime = 0.7f;
        
        [Inject]
        public TransferToken(
            SignalBus signalBus, 
            IAnimationQueueService animationQueueService, 
            IGameModuleService gameModuleService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
        }
        
        // allow
        protected override void AllowCore(TransferTokenActionDto action)
        {
            var token = GameModuleService.GetGameModuleById(action.TokenId);
            var zone = GameModuleService.GetGameModuleById(action.ZoneId);
            
            token.DragActions.Add(new DragAction(action, new [] { zone.Id }));
        }
        
        // animate
        protected override void AnimateCore(TransferTokenAnimationDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
                var token = GameModuleService.GetGameModuleById<IToken>(animation.TokenId);
                var zone = GameModuleService.GetGameModuleById<IZone>(animation.ZoneId);
                
                // remove from prev zone
                if (token.ParentZone != null)
                {
                    var parentZone = token.ParentZone;
                    parentZone.RemoveToken(token);
                    parentZone.UpdateObjectPositions();
                }

                // add to zone
                var targetTransform = zone.AddToken(token);

                // sound
                AudioService.PlaySoundEffectAsync(Globals.Instance.audioClipPlayingCard);

                // tween
                var s = DOTween.Sequence();
                s.Join(zone.UpdateObjectPositions(token));
                s.Join(token.Transform.DOMove(targetTransform.position, MoveTransitionTime));
                s.Join(token.Transform.DORotateQuaternion(targetTransform.rotation, MoveTransitionTime));
                s.Join(token.Transform.DOScale(targetTransform.localScale, MoveTransitionTime));

                s.OnComplete(() => { AnimationQueueService.Next(); });
            });
        }
    }
}