using System.ComponentModel;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Signals.Inputs;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace _src.Code.Game.Modules.Card
{
    
    public partial class Card : GameModule<CardModel>, ICard
    {
        private readonly ICameraPlaneService _cameraPlaneService;

        /// <summary>
        /// Constructor
        /// </summary>
        [Inject]
        public Card(
            CardModel model, 
            SignalBus signalBus,
            IGameHubProxy gameHubProxy,
            IGameModuleService gameModuleService,
            IInputManager inputManager, 
            ICameraPlaneService cameraPlaneService, 
            ICardFactory cardFactory) 
            : base(model, signalBus, gameModuleService, gameHubProxy, inputManager)
        {
            _cameraPlaneService = cameraPlaneService;
        }
        
        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            base.OnModelPropertyChanged(sender, args);
            
            switch (args.PropertyName)
            {
                case nameof(Model.Hand):
                    Debug.LogWarning("Change hand!!! " + Model.Id);
                    break;
            }
        }

        protected override void SubscribeToSignals()
        {
            base.SubscribeToSignals();

            Model.OnHoverStart += ShowPreview;
            Model.OnHoverEnd += HidePreview;
            
            SignalBus.Subscribe<DragStartSignal>(HidePreview);
        }

        /// <summary>
        /// Properties
        /// </summary>

        #region Properties

        public CanvasGroup CanvasGroup => Model.CanvasGroup;
        
        public IHand Hand
        {
            get => Model.Hand;
            set => Model.Hand = value;
        }
        
        public int? Slot
        {
            get => Model.Slot;
            set => Model.Slot = value;
        }

        public Image FrontImage => Model.FrontImage;
        public Image BackImage => Model.BackImage;
        public IZone ParentZone { get; set; }

        #endregion

        /// <summary>
        /// Methods
        /// </summary>
        
        #region Methods

        public override IBaseGameModule Clone()
        {
            var cardClone = (Card)GameModuleService.InstantiateGameModule<ICard>();
            
            cardClone.Model.FrontImage.sprite = Model.FrontImage.sprite;
            cardClone.BackImage.sprite = Model.BackImage.sprite;
            cardClone.Model.Text.text = Model.Text.text;
            
            cardClone.ResetInputEvents();
            
            return cardClone;
        }
        
        public void SetLayer(string sortingLayerName)
        {
            Model.Canvas.sortingLayerName = sortingLayerName;
        }

        #endregion
    }
}