using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Interfaces.UI;
using _src.Code.UI.Common;
using _src.Code.UI.Shared;
using _src.Settings;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Zenject;
using Object = UnityEngine.Object;

namespace _src.Code.App.Services
{
    public class VisualElementService : IVisualElementService
    {
        // private
        private readonly DiContainer _container;
        private readonly UIConfig _uiConfig;
        
        // Cache the two documents
        private UIDocument _uiDocument;
        private UIDocument _additiveUiDocument;
        
        // other
        private const int ShowOrHideAnimationDurationMs = 100;
        
        [Inject]
        public VisualElementService(DiContainer container, UIConfig uiConfig)
        {
            _container = container;
            _uiConfig = uiConfig;
        }
        
        // --- Helper to get the correct Root ---
        private VisualElement GetRoot(bool useAdditive)
        {
            // 1. Return Additive Root
            if (useAdditive)
            {
                if (_additiveUiDocument == null)
                {
                    var go = GameObject.Find("UIDocument_Additive"); 
                    if (go != null) _additiveUiDocument = go.GetComponent<UIDocument>();
                }

                if (_additiveUiDocument == null)
                    throw new Exception("[VisualElementService] Could not find GameObject named 'UIDocument_Additive'!");

                return _additiveUiDocument.rootVisualElement;
            }

            // 2. Return Main Root
            if (_uiDocument == null) 
            {
                var go = GameObject.Find("UIDocument"); 
                if (go != null) _uiDocument = go.GetComponent<UIDocument>();
                if (_uiDocument == null) _uiDocument = Object.FindObjectOfType<UIDocument>();
            }
            
            if (_uiDocument == null)
                throw new Exception("[VisualElementService] Could not find Main UIDocument!");
                
            return _uiDocument.rootVisualElement;
        }

        /// <summary>
        /// Finds an existing instance in the UI root, or creates a new one and adds it to the root.
        /// Use this for Screens, Popups, and global unique views.
        /// </summary>
        public async Task<T> GetOrCreate<T>(bool useAdditive = false) where T : CustomVisualElement
        {
            // 1. Determine which root we are targeting
            VisualElement targetRoot = GetRoot(useAdditive);

            // 2. SEARCH: Check if it already exists in THAT specific root
            var existingInstance = targetRoot.Q<T>(); 

            if (existingInstance != null)
            {
                // Optional: Re-inject/Re-init if needed, though usually existing instances are ready.
                // _container.Inject(existingInstance); 
                return existingInstance;
            }

            // 3. CREATE via the helper
            var newInstance = await Create<T>();

            if (newInstance != null)
            {
                // 4. ATTACH: Only GetOrCreate handles automatic root attachment
                AddToRootElement(newInstance, useAdditive);
            }

            return newInstance;
        }

        /// <summary>
        /// Always creates a new instance. Does NOT add it to the UI root.
        /// Use this for list items (e.g., PlayerRow) that you will manually add to a container.
        /// </summary>
        public async Task<T> Create<T>() where T : CustomVisualElement
        {
            // 1. Load from Config
            var assetRef = _uiConfig.GetAssetReference<T>();
            if (assetRef == null)
            {
                Debug.LogError($"[UI] No config entry found for '{typeof(T).Name}'");
                return null;
            }

            // 2. Load Asset
            var visualTree = await Addressables.LoadAssetAsync<VisualTreeAsset>(assetRef).Task;

            // 3. Instantiate & Setup
            T newInstance = Activator.CreateInstance<T>(); 
            _container.Inject(newInstance);      // DI Injection
            newInstance.SetVisualTree(visualTree); 
            newInstance.Initialize();            // Custom Init logic

            return newInstance;
        }
        
        /// <summary>
        /// check if a visualElement is inside rootElement
        /// </summary>
        public bool DocumentContains(VisualElement visualElement, bool useAdditive = false)
        {
            var targetRoot = GetRoot(useAdditive);
            if (targetRoot != null)
            {
                return targetRoot.Contains(visualElement);
            }

            return false;
        }
        
        /// <summary>
        /// add a visualElement to the root
        /// </summary>
        public bool AddToRootElement(VisualElement visualElement, bool useAdditive = false)
        {
            var targetRoot = GetRoot(useAdditive);

            if (targetRoot != null)
            {
                targetRoot.Add(visualElement);
                
                visualElement.style.position = Position.Absolute;
                
                visualElement.style.top = 0;
                visualElement.style.bottom = 0;
                visualElement.style.left = 0;
                visualElement.style.right = 0;
                
                visualElement.style.flexGrow = 1;
                visualElement.style.width = Length.Percent(100);
                visualElement.style.height = Length.Percent(100);
                
                visualElement.BringToFront();
                
                return true;
            }
            else
            {
                Debug.LogError("UIDocument not found in the scene.");
                return false;
            }
        }
        
        public Sprite GetSprite<T>(string key)
        {
            return _uiConfig.GetSprite<T>(key);
        }
        
        public Sprite GetSprite(string typeName, string key)
        {
            return _uiConfig.GetSprite(typeName, key); 
        }
        
        // BLUR
        
        private Blur activeBlur;
        private ValueAnimation<StyleValues> valueanimation;

        public async Task AddBlur()
        {
            // // stop current animation
            // if (valueanimation != null)
            // {
            //     valueanimation.Stop();
            // }
            //
            // // Ensure there is only one blur element
            // if (activeBlur == null)
            // {
            //     activeBlur = await GetOrCreate<Blur>();
            // }
            //
            // if (!DocumentContains(activeBlur))
            // {
            //     // Fade in
            //     activeBlur.AddToRootElement();
            //     activeBlur.style.opacity = 0f;
            //     valueanimation = activeBlur.experimental.animation
            //         .Start(new StyleValues { opacity = 1f }, ShowOrHideAnimationDurationMs)
            //         .OnCompleted(() => valueanimation = null);
            // }
        }

        public async Task RemoveBlur()
        {
            if (activeBlur != null && DocumentContains(activeBlur))
            {
                var blur = activeBlur;

                // Fade out
                valueanimation = blur.experimental.animation
                    .Start(new StyleValues { opacity = 0f }, ShowOrHideAnimationDurationMs)
                    .OnCompleted(() =>
                    {
                        blur.RemoveFromHierarchy();
                        valueanimation = null;
                        
                        if (activeBlur == blur) 
                            activeBlur = null;
                    });
            }
        }
        
        // SHOW / HIDE
        
        public void Show(VisualElement visualElement)
        {
            if (visualElement != null)
            {
                // animate entry
                visualElement.style.scale = new StyleScale(new Vector2(0f, 0f));
                visualElement.experimental.animation.Start(
                    (e) => e.transform.scale,
                    new Vector3(1, 1, 1),
                    ShowOrHideAnimationDurationMs,
                    (e, v) => { e.transform.scale = v; });
            }
        }
        
        public void Hide(VisualElement visualElement, Action callback = null)
        {
            if (visualElement != null)
            {
                // animate exit
                visualElement.experimental.animation.Start(
                    (e) => e.transform.scale,
                    new Vector3(0, 0, 0),
                    ShowOrHideAnimationDurationMs,
                    (e, v) => { e.transform.scale = v; }).OnCompleted(() =>
                {
                    callback?.Invoke();
                });
            }
        }

        public async Task ShowWarning(string message)
        {
            var warning = await Create<WarningPopup>();
            warning.Title.Label.text = "Warning!";
            warning.Message.text = message;
            warning.Button.Label.text = "Ok";
            
            warning.AddToRootElement();
        }
    }
}