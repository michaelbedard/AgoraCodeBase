using System.Collections.ObjectModel;
using _src.Code.Core.Interfaces.GameModules;
using Agora.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Game.Modules.Zone
{
    public class ZoneModel : GameModuleModel
    {
        [Header("Zone")]
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private Image background;
        [SerializeField]
        private Image backgroundGlow;
        
        private ObservableCollection<ICard> _cards = new ObservableCollection<ICard>();
        private ObservableCollection<IToken> _tokens = new ObservableCollection<IToken>();
        private ObservableCollection<IMarker> _markers = new ObservableCollection<IMarker>();
        
        /// <summary>
        /// Constructor
        /// </summary>
        public ZoneModel()
        {
            Type = GameModuleType.Zone;
        }

        /// <summary>
        /// View Properties
        /// </summary>
        
        public Canvas Canvas => canvas;
        public Image Background => background;
        public Image BackgroundGlow => backgroundGlow;
        
        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public ObservableCollection<ICard> Cards
        {
            get => _cards;
            set
            {
                if (_cards != value)
                {
                    _cards = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<IToken> Tokens
        {
            get => _tokens;
            set
            {
                if (_tokens != value)
                {
                    _tokens = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public ObservableCollection<IMarker> Markers
        {
            get => _markers;
            set
            {
                if (_markers != value)
                {
                    _markers = value;
                    OnPropertyChanged();
                }
            }
        }

        public ZoneStackingMethod StackingMethod
        {
            get;
            set;
        }
    }
}