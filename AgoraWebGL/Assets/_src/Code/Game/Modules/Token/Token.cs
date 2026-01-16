using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Modules.Token
{
    
    public class Token : GameModule<TokenModel>, IToken
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Token(
            TokenModel model,
            SignalBus signalBus, 
            IGameModuleService gameModuleService, 
            IGameHubProxy gameHubProxy,
            IInputManager inputManager) 
            : base(model, signalBus, gameModuleService, gameHubProxy, inputManager)
        {
        }
        
        /// <summary>
        /// Properties
        /// </summary>
        #region Properties

        

        #endregion
        
        /// <summary>
        /// Methods
        /// </summary>
        
        public override IBaseGameModule Clone()
        {
            return null;
            // if (otherGameModule is not IToken zone)
            //     return;
            //
            // var c = zone.Model.GlowImage;
        }

        public IZone ParentZone { get; set; }

        public void SetLabel(string label)
        {
            throw new System.NotImplementedException();
        }

        public void SetColor(Color color)
        {
            Model.Material.color = color;
            Model.Color = color;
        }
    }
}