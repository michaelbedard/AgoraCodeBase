using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using UnityEngine.UIElements;
using Zenject;

namespace _src.Code.Core.Actors
{
    public abstract class CustomVisualElement: VisualElement
    {
        private IVisualElementService _visualElementService;
        protected IVisualElementService VisualElementService => 
            _visualElementService ??= ServiceLocator.GetService<IVisualElementService>();

        private IInputManager _inputManager;
        private IInputManager InputManager => 
            _inputManager ??= ServiceLocator.GetService<IInputManager>();
        
        private bool _isInitialized;
        private bool _isMouseOver;
            
        protected abstract void InitializeCore();

        public void Initialize()
        {
            if (_isInitialized) return;
            
            _isInitialized = true;

            RegisterInputCallbacks();
            InitializeCore(); // Child class logic
        }
        
        // Allow passing the UXML asset if needed
        public void SetVisualTree(VisualTreeAsset tree)
        {
            tree.CloneTree(this);
        }
        
        private void RegisterInputCallbacks()
        {
            RegisterCallback<MouseEnterEvent>(evt => InputManager.MouseOverUI = true);
            RegisterCallback<MouseLeaveEvent>(evt => InputManager.MouseOverUI = false);
        }
        

        /// <summary>
        /// Call Initialize on geometry changed.  Useful for root components
        /// </summary>
        protected CustomVisualElement()
        {
            RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
        }
        
        private void OnGeometryChanged(GeometryChangedEvent evt)
        {
            if (!Application.isPlaying)
            {
                return;
            }
        
            UnregisterCallback<GeometryChangedEvent>(OnGeometryChanged);
            Initialize();
        }
        
        #region Utility methods

        /// <summary>
        /// Add this to the root element
        /// </summary>
        public void AddToRootElement()
        {
            var success = VisualElementService.AddToRootElement(this);
            if (success)
            {
                OnAddToRootElement();
            }
        }

        protected virtual void OnAddToRootElement()
        {
            // nothing
        }
        
        /// <summary>
        /// call this.Q, then call initialize on the component if is CustomVisualElement
        /// </summary>
        public VisualElement Get(string elementName = null, string className = null)
        {
            var e = this.Q(elementName, className);
            if (e is CustomVisualElement customVisualElement)
            {
                customVisualElement.Initialize();
            }

            return e;
        }
        
        /// <summary>
        /// call this.Q<>, then call initialize on the component if is CustomVisualElement
        /// </summary>
        public T Get<T>(string elementName = null, string className = null) where T : VisualElement
        {
            // 1. Try finding it directly (Standard Unity behavior)
            var e = this.Q<T>(elementName, className);

            // 2. THE FIX: If not found, and we searched by name, check if that name belongs to a Wrapper
            if (e == null && !string.IsNullOrEmpty(elementName))
            {
                var wrapper = this.Q(elementName, className); // Find the wrapper (TemplateContainer)
                if (wrapper != null)
                {
                    e = wrapper.Q<T>(); // Look for the Type inside the Wrapper
                }
            }
    
            // 3. Initialize if found
            if (e is CustomVisualElement customVisualElement && customVisualElement != this)
            {
                customVisualElement.Initialize();
            }

            return e;
        }
        
        /// <summary>
        /// Finds the first parent of the specified type in the hierarchy.
        /// </summary>
        public T FindParentOfType<T>() where T : VisualElement
        {
            var current = parent;
        
            while (current != null)
            {
                if (current is T parentOfType)
                {
                    return parentOfType;
                }

                current = current.parent;
            }

            return null;
        }
        
        // animations
        
        private bool _withBlur;

        public async void Show(bool withBlur = true)
        {
            _withBlur = withBlur;
            if (withBlur)
            {
                Globals.Instance.BlurCount += 1;
                await VisualElementService.AddBlur();
            }
            
            AddToRootElement();
            VisualElementService.Show(this);
        }
        
        public async void Hide()
        {
            if (_withBlur)
            {
                Globals.Instance.BlurCount -= 1;
                if (Globals.Instance.BlurCount == 0)
                {
                    await VisualElementService.RemoveBlur();
                }
            }
            
            VisualElementService.Hide(this, RemoveFromHierarchy);
        }

        #endregion
    }
}