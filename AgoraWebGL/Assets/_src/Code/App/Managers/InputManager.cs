using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace _src.Code.App.Managers
{
    public partial class InputManager : UserInputs.IGamePlayActions, IInputManager, ITickable
    {
        // properties
        private SignalBus SignalBus => ServiceLocator.GetService<SignalBus>();
        public List<InputObject> InputObjectsAtMousePosition => _objectListAtMousePosition;
        public bool MouseOverUI { get; set; }
        public bool IsDragging => _isDragging;
        
        // private fields
        private readonly UserInputs _gameInput;
        private bool _isClicking;
        
        [Inject]
        public InputManager(SignalBus signalBus)
        {
            _gameInput = new UserInputs();
            
            _gameInput.GamePlay.SetCallbacks(this);
            _gameInput.GamePlay.SetCallbacks(this);

            EnableGamePlay();
        }

        public void EnableGamePlay()
        {
            _gameInput.GamePlay.Enable();
        }
        
        public void DisableGamePlay()
        {
            _gameInput.GamePlay.Disable();
        }
        
        public void Tick()
        {
            UpdateObjectAtMousePosition();
            
            if (_isClicking)
            {
                // Continuously call OnClickPerformed as long as the click is held down
                OnClickUpdate();
            }

            // update cursor
            UpdateCursor();
        }

        public void OnClick(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                OnClickStarted();
                _isClicking = true;
            }
            if (context.canceled)
            {
                OnClickCanceled();
                _isClicking = false;
            }
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            var mousePosition = context.ReadValue<Vector2>();
            OnMouseMove(mousePosition);
        }
        
        public void OnScroll(InputAction.CallbackContext context)
        {
            var scrollDelta = context.ReadValue<Vector2>(); // Scroll is often represented as a Vector2
            OnScroll(scrollDelta);
        }
    }
}