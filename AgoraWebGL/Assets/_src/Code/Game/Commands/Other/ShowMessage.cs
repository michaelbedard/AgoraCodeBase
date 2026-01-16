using System;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using _src.Code.UI.Scenes.Game;
using Agora.Core.Dtos.Game.Commands.Other;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Commands.Other
{
    public class ShowMessage : BaseAnimation<ShowMessageCommandDto>
    {
        private readonly IGameDataService _gameDataService;
        private readonly IVisualElementService _visualElementService;
        
        public ShowMessage(
            SignalBus signalBus,
            IAnimationQueueService animationQueueService,
            IGameModuleService gameModuleService,
            IGameDataService gameDataService,
            IVisualElementService visualElementService) 
            : base(signalBus, animationQueueService, gameModuleService)
        {
            _gameDataService = gameDataService;
            _visualElementService = visualElementService;
        }

        protected override void AnimateCore(ShowMessageCommandDto animation)
        {
            AnimationQueueService.Push(async () =>
            {
                try
                {
                    var durationMillis = animation.DurationMillis ?? 2000;
                    
                    if (animation.BlockSequence)
                    {
                        await ShowMessageAsync(animation.Message, durationMillis);
                        AnimationQueueService.Next();
                    }
                    else
                    {
                        AnimationQueueService.Next();
                        await ShowMessageAsync(animation.Message, durationMillis);
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            });
        }

        private async Task ShowMessageAsync(string message, int durationMillis)
        {
            var p = await _visualElementService.GetOrCreate<PlayerTurnPopup>();
            p.Message = message;
            p.Show(false);
            
            // wait
            await Task.Delay(durationMillis);
            
            // hide
            if (_visualElementService.DocumentContains(p))
            {
                p.Hide();
            }
        }
    }
}