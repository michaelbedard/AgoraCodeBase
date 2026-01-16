using System;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IVisualElementService
    {
        Task<T> GetOrCreate<T>(bool useAdditive = false) where T : CustomVisualElement;
        Task<T> Create<T>() where T : CustomVisualElement;
        Sprite GetSprite<T>(string key);
        Sprite GetSprite(string typeName, string key);
        bool DocumentContains(VisualElement visualElement, bool useAdditive = false);
        Task AddBlur();
        Task RemoveBlur();
        bool AddToRootElement(VisualElement visualElement, bool useAdditive = false);
        void Show(VisualElement visualElement);
        void Hide(VisualElement visualElement, Action callback = null);
    }
}