using System.Collections.ObjectModel;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Modules.Zone
{
    public partial class Zone : GameModule<ZoneModel>, IZone
    {
        /// <summary>
        /// Constructor
        /// </summary>
        [Inject]
        public Zone(
            ZoneModel model, 
            SignalBus signalBus, 
            IGameModuleService gameModuleService, 
            IGameHubProxy gameHubProxy,
            IInputManager inputManager) 
            : base(model, signalBus, gameModuleService, gameHubProxy, inputManager)
        {
            Type = GameModuleType.Zone;
        }

        /// <summary>
        /// Properties
        /// </summary>
        #region Properties

        public Canvas Canvas => Model.Canvas;
        
        public ObservableCollection<ICard> Cards
        {
            get => Model.Cards;
            set => Model.Cards = value;
        }
        
        public ObservableCollection<IToken> Tokens
        {
            get => Model.Tokens;
            set => Model.Tokens = value;
        }
        
        public ObservableCollection<IMarker> Markers
        {
            get => Model.Markers;
            set => Model.Markers = value;
        }
        
        public ZoneStackingMethod StackingMethod
        {
            get => Model.StackingMethod;
            set => Model.StackingMethod = value;
        }

        #endregion

        /// <summary>
        /// Methods
        /// </summary>
        #region Methods

        public override IBaseGameModule Clone()
        {
            return null;
            // if (otherGameModule is not Zone zone)
            //     return;
            //
            // var c = zone.Model.GlowImage;
        }
        
        public override void EnableGlow()
        {
            base.EnableGlow();
            
            // same for frame
            if (Model.BackgroundGlow == null)
                return;
            
            var currentAlpha = Model.BackgroundGlow.color.a;
            var glowColor = Color.green;
            
            // TODO check if special color
            
            glowColor.a = currentAlpha; 
            Model.BackgroundGlow.gameObject.SetActive(true);
            Model.BackgroundGlow.color = glowColor;
        }

        public override void DisableGlow()
        {
            base.DisableGlow();
            
            if (Model.BackgroundGlow != null)
                Model.BackgroundGlow.gameObject.SetActive(false);
        }

        #endregion

    }
}