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
    public class ActivateCard : BaseAction<ActivateCardActionDto, ActivateCardAnimationDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IParticleSystemService _particleSystemService;
        
        [Inject]
        public ActivateCard(
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
        protected override void AllowCore(ActivateCardActionDto action)
        {
            var card = GameModuleService.GetGameModuleById(action.CardId);
            card.ClickActions.Add(new ClickAction(action));

            card.CanBeClick = true;
        }
        
        // animate
        protected override void AnimateCore(ActivateCardAnimationDto animation)
        {
            AnimationQueueService.Push(() =>
            {
                var card = GameModuleService.GetGameModuleById<ICard>(animation.CardId);

                if (animation.ShouldAnimate)
                {
                    // animate
                    var s = DOTween.Sequence();
                    s.JoinCallback(() => 
                    {
                        // particle animation
                        _particleSystemService.Play(card.Transform);
                    });

                    s.OnComplete(() =>
                    {
                        AnimationQueueService.Next();
                    });
                }
                else
                {
                    AnimationQueueService.Next();
                }
            });
        }
    }
}