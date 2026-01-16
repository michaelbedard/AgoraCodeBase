using System.Collections.ObjectModel;
using _src.Code.Core.Interfaces.GameModules;
using Agora.Core.Enums;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Game.Modules.Deck
{
    public class DeckModel : GameModuleModel
    {
        [Header("Deck")]
        [SerializeField] private Image backImage;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private Transform topDeckTransform;
        
        private Material _material;
        private ObservableCollection<ICard> _cards;
        private bool _isBouncing;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DeckModel()
        {
            Type = GameModuleType.Deck;
        }

        /// <summary>
        /// View Properties
        /// </summary>
        
        public Image BackImage => backImage;
        public MeshRenderer MeshRenderer => meshRenderer;
        public Transform TopDeckTransform => topDeckTransform;
        
        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public Material Material
        {
            get => _material;
            set
            {
                if (_material != value)
                {
                    _material = value;
                    OnPropertyChanged();
                }
            }
        }
        
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
        
        public bool IsBouncing
        {
            get => _isBouncing;
            set
            {
                if (_isBouncing != value)
                {
                    _isBouncing = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}