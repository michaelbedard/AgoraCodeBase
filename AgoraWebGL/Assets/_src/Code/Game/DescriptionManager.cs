using System;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Game.Modules;
using _src.Code.Game.Modules.Card;
using _src.Code.UI.Scenes.Game;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.Game
{
    public class DescriptionManager : IDescriptionManager, ITickable
    {
        private readonly IInputManager _inputManager;
        private readonly IVisualElementService _visualElementService;
        private readonly IGameModuleService _gameModuleService;
        private readonly IGameDataService _gameDataService;

        private bool _isRunning;
        private bool _panelIsShowing;
        private Description _description;
        
        [Inject]
        public DescriptionManager(
            IInputManager inputManager, 
            IVisualElementService visualElementService, 
            IGameDataService gameDataService,
            IGameModuleService gameModuleService)
        {
            _inputManager = inputManager;
            _visualElementService = visualElementService;
            _gameDataService = gameDataService;
            _gameModuleService = gameModuleService;
        }
        
        public async Task Start()
        {
            var gameScreen = await _visualElementService.GetOrCreate<GameScreen>();
            _description = gameScreen.Description;
            
            _description.Title = string.Empty;
            _description.Content = string.Empty;
            _description.parent.style.display = DisplayStyle.Flex;
            
            _visualElementService.Hide(_description.parent);
            
            _isRunning = true;
            _panelIsShowing = false;
        }
        
        public void Stop()
        {
            _description.Content = string.Empty;
            _description.parent.style.display = DisplayStyle.None;
            
            _isRunning = false;
            _panelIsShowing = false;
        }
        
        public void Tick()
        {
            if (!_isRunning || _description == null)
            {
                return;
            }

            if (_inputManager.IsDragging || _inputManager.MouseOverUI)
            {
                HidePanel();
                return;
            }
            
            var inputObjectsAtMousePosition = _inputManager.InputObjectsAtMousePosition;

            foreach (var inputObject in inputObjectsAtMousePosition)
            {
                if (inputObject is GameModuleModel model)
                {
                    // ignore if no description
                    if (string.IsNullOrWhiteSpace(model.Description))
                    {
                        // unless it's opponent hand
                        if (inputObject is CardModel card && card.Hand != null &&
                            _gameDataService.PlayerId != card.Hand.PlayerId)
                        {
                            var player = _gameModuleService.GetGameModuleById(card.Hand.PlayerId);
                            if (!string.IsNullOrWhiteSpace(player.Description))
                            {
                                // show opponent description
                                ShowPanel(player.Name, player.Description);
                                return;
                            }
                        }
                        
                        continue;
                    }
                
                    // show module description
                    ShowPanel(model.Name, model.Description);
                    return;
                }
            }
            
            HidePanel();
        }
        
        private void ShowPanel(string title, string description)
        {
            if (!_panelIsShowing)
            {
                _visualElementService.Show(_description.parent);
                _panelIsShowing = true;
            }
            
            _description.Title = title;
            _description.Content = description;
        }

        private void HidePanel()
        {
            if (_panelIsShowing)
            {
                _visualElementService.Hide(_description.parent);
                _panelIsShowing = false;
            }
            
            _description.Title = string.Empty;
            _description.Content = string.Empty;
        }
    }
}