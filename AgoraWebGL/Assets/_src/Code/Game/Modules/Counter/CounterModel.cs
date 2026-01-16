using Agora.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Game.Modules.Counter
{
    public class CounterModel : GameModuleModel
    {
        [Header("Counter")]
        public TextMeshProUGUI text;
        public Image backgroundImage;

        private int _value;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public CounterModel()
        {
            Type = GameModuleType.Counter;
        }
        
        /// <summary>
        /// View Properties
        /// </summary>
        
        public TextMeshProUGUI Text => text;
        public Image BackgroundImage => backgroundImage;
        
        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public int Value
        {
            get => _value;
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}