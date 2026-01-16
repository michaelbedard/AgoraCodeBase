using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.UI;
using UnityEngine.UIElements;

namespace _src.Code.UI.Common
{
    public class Blur : CustomVisualElement, IVisualElement
    {
        public new class UxmlFactory : UxmlFactory<Blur, UxmlTraits>
        {
        }

        protected override void InitializeCore()
        {
        }
    }
}