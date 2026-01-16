using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Game.Modules.Card.Other;
using Agora.Core.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _src.Code.Game.Modules.Card
{
    public class CardModel : GameModuleModel
    {
        [Header("Card")]
        [SerializeField] private CardRotation cardRotation;
        [SerializeField] private CanvasGroup canvasGroup;
        [SerializeField] private Canvas canvas;
        [SerializeField] private Image frontImage;
        [SerializeField] private Image backImage;
        [SerializeField] private TextMeshProUGUI text;

        private IHand _hand;
        private int? _slot;

        /// <summary>
        /// Constructor
        /// </summary>
        public CardModel()
        {
            Type = GameModuleType.Card;
        }

        /// <summary>
        /// View Properties
        /// </summary>

        public CanvasGroup CanvasGroup => canvasGroup;
        public Canvas Canvas => canvas;
        public Image FrontImage => frontImage;
        public Image BackImage => backImage;
        public TextMeshProUGUI Text => text;

        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public IHand Hand
        {
            get => _hand;
            set
            {
                Debug.Log(Id);
                Debug.Log(value);
                
                if (_hand != value)
                {
                    _hand = value;
                    OnPropertyChanged();
                }
            }
        }

        public int? Slot
        {
            get => _slot;
            set
            {
                if (_slot != value)
                {
                    _slot = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowingBack => cardRotation.showingBack;
    }
}