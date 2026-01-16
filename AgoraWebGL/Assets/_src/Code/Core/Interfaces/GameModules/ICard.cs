using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules.Other;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface ICard : IBaseGameModule
    {
        // Properties
        CanvasGroup CanvasGroup { get; }
        IHand Hand { get; set; }
        public int? Slot { get; set; }
        Image FrontImage { get; }
        Image BackImage { get; }
        IZone ParentZone { get; set; }

        // Methods
        void SetLayer(string sortingLayerName);
        void HidePreview();
    }
}