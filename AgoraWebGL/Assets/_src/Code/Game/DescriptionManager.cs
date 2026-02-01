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
        [Inject]
        private readonly IClientDataService _clientDataService;

        private bool _isRunning;
        private bool _panelIsShowing;
        private Description _description;
        
        [Inject]
        public DescriptionManager(
            IInputManager inputManager, 
            IVisualElementService visualElementService, 
            IGameModuleService gameModuleService)
        {
            _inputManager = inputManager;
            _visualElementService = visualElementService;
            _gameModuleService = gameModuleService;
        }
        
        public async Task Start()
        {
            _description = await _visualElementService.GetOrCreate<Description>();
            
            _visualElementService.AddToRootElement(_description.parent);
            _description.parent.pickingMode = PickingMode.Ignore;
            _description.parent.focusable = false;
            
            _description.Container.transform.scale = Vector3.zero;
            
            _isRunning = true;
            _panelIsShowing = false;
        }
        
        public void Stop()
        {
            HidePanel();
            
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
                        if (inputObject is CardModel card && card.Hand != null && _clientDataService.Id != card.Hand.PlayerId)
                        {
                            if (!string.IsNullOrWhiteSpace(card.Hand.Description))
                            {
                                var playerUsername = _clientDataService.Players.Find(p => p.Id == card.Hand.PlayerId).Username;
                                ShowPanel(playerUsername, card.Hand.Description);
                                
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
                _visualElementService.Show(_description.Container);
                _panelIsShowing = true;
            }
            
            _description.Setup(title, description);
        }

        private void HidePanel()
        {
            if (_panelIsShowing)
            {
                _visualElementService.Hide(_description.Container);
                _panelIsShowing = false;
            }
        }
    }
}