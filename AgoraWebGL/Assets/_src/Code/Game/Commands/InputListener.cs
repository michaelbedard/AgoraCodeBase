using System;
using System.Collections.Generic;
using System.Linq;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Game;
using _src.Code.Game.Commands.GameInputs;
using Agora.Core.Dtos.Game.Commands;
using Agora.Core.Dtos.Game.Commands.Inputs;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Commands
{
    public class InputListener 
    {
        private readonly IAnimationQueueService _animationQueueService;
        private readonly IGameModuleService _gameModuleService;

        private List<BaseInput> _inputs;

        [Inject]
        protected InputListener(SignalBus signalBus, IAnimationQueueService animationQueueService, IGameModuleService gameModuleService)
        {
            _animationQueueService = animationQueueService;
            _gameModuleService = gameModuleService;
            _inputs = new List<BaseInput>();
            
            signalBus.Subscribe<GameInputSignal>(s => DoInput(s.Input));
        }

        private void DoInput(CommandDto commandDto)
        {
            if (_inputs.Any(i => i.InputId == commandDto.Id))
            {
                return;
            }
            
            BaseInput input = commandDto switch
            {
                ChoiceInputDto => new ChooseInput(),
                SelectAmountDto => new SelectAmountInput(),
                ButtonInputDto => new ButtonInput(),
                _ => throw new ArgumentException($"Unsupported input type: {commandDto.GetType().Name}")
            };

            input.InputId = commandDto.Id;
            input.SignalBus = ServiceLocator.GetService<SignalBus>();
            input.GameModuleService = _gameModuleService;
            input.AnimationQueueService = _animationQueueService;
            
            // cancel all prev
            foreach (var prevInput in _inputs)
            {
                prevInput.Cancel();
            }
            
            _inputs.Add(input);
            input.Ask(commandDto);
        }
    }
}