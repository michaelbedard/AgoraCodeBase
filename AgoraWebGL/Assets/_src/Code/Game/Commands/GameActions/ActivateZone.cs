using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.Commands.Actions;

using DG.Tweening;
using Zenject;

namespace _src.Code.Game.Commands.GameActions
{
    public class ActivateZone : BaseAction<ActivateZoneActionDto, ActivateZoneAnimationDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IParticleSystemService _particleSystemService;
        
        [Inject]
        public ActivateZone(
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
        protected override void AllowCore(ActivateZoneActionDto action)
        {
            var zone = GameModuleService.GetGameModuleById(action.ZoneId);
            zone.ClickActions.Add(new ClickAction(action));

            zone.CanBeClick = true;
        }
        
        // animate
        protected override void AnimateCore(ActivateZoneAnimationDto animation)
        {
            AnimationQueueService.Push(() =>
            {
                var zone = GameModuleService.GetGameModuleById<IZone>(animation.ZoneId);

                if (animation.ShouldAnimate)
                {
                    // animate
                    var s = DOTween.Sequence();
                    s.JoinCallback(() => 
                    {
                        // particle animation
                        _particleSystemService.Play(zone.Transform);
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