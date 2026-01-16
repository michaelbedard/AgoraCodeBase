using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Inputs;
using _src.Code.UI.Scenes.Additive.SettingsWindow;
using Agora.Core.Dtos.Game.Commands.Inputs;
using Agora.Core.Payloads.Hubs;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using Zenject.SpaceFighter;

namespace _src.Code.Game.Commands.GameInputs
{
    public class ChooseInput : BaseInput<ChoiceInputDto>
    {
        private readonly ICameraPlaneService _cameraPlaneService;
        private readonly IVisualElementService _visualElementService;
        private readonly IGameHubProxy _gameHubProxy;
        
        // private ShopPanel _shopPanel;
        private Action<ClickSignal> _clickSignalHandler;
        private bool _isShowingOptions;
        private readonly List<IBaseGameModule> _choices = new List<IBaseGameModule>();
        
        private const float Distance = 1.0f;
        
        public ChooseInput()
        {
            _cameraPlaneService = ServiceLocator.GetService<ICameraPlaneService>();
            _visualElementService = ServiceLocator.GetService<IVisualElementService>();
            _gameHubProxy = ServiceLocator.GetService<IGameHubProxy>();;
        }
        
        // ask
        protected override async void AskCore(ChoiceInputDto input)
        {
            // set label
            // _shopPanel = await _visualElementService.GetOrCreate<ShopPanel>();
            // _shopPanel.Label = input.Label;
            // _shopPanel.SecondaryButton.Clicked += () =>
            // {
            //     if (_isShowingOptions)
            //     {
            //         HideOptions();
            //     }
            //     else
            //     {
            //         ShowOptions();
            //     }
            // };
            //
            // _shopPanel.Show(false);
            // ShowOptions();
            
            // set choices
            var numberOfChoices = input.Choices.Length;
            for (var i = 0; i < numberOfChoices; i++)
            {
                var choice = input.Choices[i];
                IBaseGameModule choiceIBaseController;
                
                if (choice is GameModuleChoiceDto gameModuleChoiceDto)
                {
                    choiceIBaseController = GameModuleService.GetGameModuleById(gameModuleChoiceDto.GameModuleId).Clone();

                    // additional config
                    if (choiceIBaseController is Player player)
                    {
                        // player.HidePodium();
                        // player.Transform.localScale *= 2;
                    }
                }
                else if (choice is TextChoiceDto textChoiceDto)
                {
                    choiceIBaseController = GameModuleService.InstantiateGameModule<ICard>();

                    try
                    {
                        ((ICard)choiceIBaseController).FrontImage.sprite =
                            await Addressables.LoadAssetAsync<Sprite>(textChoiceDto.CardBackgroundAsset).Task;
                    }
                    catch (Exception e)
                    {
                        Debug.LogException(e);
                    }
                }
                else
                {
                    throw new Exception("Invalid choice type");
                }
                
                // set layer
                if (choiceIBaseController is ICard card)
                {
                    card.SetLayer("AboveEverything");
                }
                
                // position
                var xPosition = numberOfChoices > 1 
                    ? i * Distance - Distance * (numberOfChoices - 1) / 2 
                    : 0; // Center the single choice
                
                var yPosition = choiceIBaseController is Player ? -0.15f : 0;

                _cameraPlaneService.PositionElement(choiceIBaseController.Transform, new Vector2(xPosition, yPosition), tangentialRotation: 180f, offset:-5f);
                _choices.Add(choiceIBaseController);
            }
            
            if (_clickSignalHandler != null)
            {
                SignalBus.Unsubscribe(_clickSignalHandler);
            }
            _clickSignalHandler = async s => await CheckAndResolveInput(s);
            SignalBus.Subscribe<ClickSignal>(_clickSignalHandler);
        }

        private void ShowOptions()
        {
            // _shopPanel.SubTitle.style.display = DisplayStyle.Flex;
            // foreach (var choice in _choices)
            // {
            //     choice.Transform.gameObject.SetActive(true);
            // }
            //
            // _shopPanel.SecondaryButton.Label.text = "Hide";
            // _isShowingOptions = true;
        }
        
        private void HideOptions()
        {
            // _shopPanel.SubTitle.style.display = DisplayStyle.None;
            // foreach (var choice in _choices)
            // {
            //     choice.Transform.gameObject.SetActive(false);
            // }
            //
            // _shopPanel.SecondaryButton.Label.text = "Show";
            // _isShowingOptions = false;
        }

        private async Task CheckAndResolveInput(ClickSignal signal)
        {
            for (var i = 0; i < _choices.Count; i++)
            {
                if (signal.InputObject != _choices[i].InputObject)
                    continue;
                
                // resolve choice
                Debug.Log($"DO CHOICE #{i}");
            
                await _gameHubProxy.ExecuteInput(new ExecuteInputPayload()
                {
                    InputId = InputId,
                    Answer = i
                });
            
                foreach (var gameModule in _choices)
                {
                    gameModule.Destroy();
                }

                // _shopPanel.Hide();
            
                if (_clickSignalHandler != null)
                {
                    SignalBus.Unsubscribe(_clickSignalHandler);
                    _clickSignalHandler = null;
                }
                return;
            }
        }
        
        // cancel
        public override void Cancel()
        {
            if (_clickSignalHandler != null)
            {
                SignalBus.TryUnsubscribe(_clickSignalHandler);
                _clickSignalHandler = null; // Ensure it doesn't unsubscribe again
            }
            
            // _shopPanel.Hide();
            
            foreach (var choice in _choices)
            {
                choice.Destroy();
            }
        }
    }
}