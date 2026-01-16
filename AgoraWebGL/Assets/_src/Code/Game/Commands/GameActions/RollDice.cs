using System;
using _src.Code.Core;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Actions;

using DG.Tweening;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

namespace _src.Code.Game.Commands.GameActions
{
    public class RollDice : BaseAction<RollDiceActionDto, RollDiceAnimationDto>
    {
        const float LaunchHeight = 2f;
        const float RollDuration = 1f;
        
        [Inject]
        public RollDice(
            SignalBus signalBus, 
            IAnimationQueueService animationQueueService, 
            IGameModuleService gameModuleService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
        }
        
        // allow
        protected override void AllowCore(RollDiceActionDto action)
        {
            var dice = GameModuleService.GetGameModuleById(action.DiceId);
            dice.ClickActions.Add(new ClickAction(action));
        }
        
        // animate
        protected override void AnimateCore(RollDiceAnimationDto animation)
        {
            AnimationQueueService.Push(() =>
            {
                var dice = GameModuleService.GetGameModuleById<IDice>(animation.DiceId);
                var diceTransform = dice.DiceObject.transform;
                
                var finalRotation = GetRotationForRollResult(animation.RollResult);
                
                // sound
                ServiceLocator.GetService<IAudioService>().PlaySoundEffectAsync(Globals.Instance.audioClipRollingDice);

                // tween
                var s = DOTween.Sequence();
                s.Append(diceTransform.DOMoveY(diceTransform.position.y + LaunchHeight, RollDuration / 2)
                    .SetEase(Ease.OutQuad));
                s.Join(diceTransform.DORotate(new Vector3(
                    Random.Range(0, 360),
                    Random.Range(0, 360),
                    Random.Range(0, 360)), RollDuration / 2, RotateMode.FastBeyond360));
                s.Append(diceTransform.DOMoveY(diceTransform.position.y, RollDuration / 2)
                    .SetEase(Ease.InQuad));
                s.Join(diceTransform.DORotate(finalRotation, RollDuration / 2, RotateMode.FastBeyond360));

                s.OnComplete(() =>
                {
                    AnimationQueueService.Next();
                });
            });
        }
        
        private Vector3 GetRotationForRollResult(int rollResult)
        {
            // Map the dice face to its world-space rotation (adjust for your dice model)
            switch (rollResult)
            {
                case 1: return new Vector3(0, 0, 0);
                case 2: return new Vector3(0, 0, 90);
                case 3: return new Vector3(0, 0, 270);
                case 4: return new Vector3(90, 0, 0);
                case 5: return new Vector3(270, 0, 0);
                case 6: return new Vector3(180, 0, 0);
                default: throw new ArgumentOutOfRangeException(nameof(rollResult), "Invalid roll result");
            }
        }
    }
}