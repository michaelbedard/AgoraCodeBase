using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Modules.Dice
{
    public class Dice : GameModule<DiceModel>, IDice
    {
        public Dice(
            DiceModel model, 
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
        
        public int Number { get; set; }

        public GameObject DiceObject => Model.DiceObject;

        /// <summary>
        /// Methods
        /// </summary>
        public override IBaseGameModule Clone()
        {
            // TODO

            return null;
        }

    }
}