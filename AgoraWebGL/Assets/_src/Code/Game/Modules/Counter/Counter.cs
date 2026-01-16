using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using Zenject;

namespace _src.Code.Game.Modules.Counter
{
    public partial class Counter : GameModule<CounterModel>, ICounter
    {
        /// <summary>
        /// Constructor
        /// </summary>
        [Inject]
        public Counter(
            CounterModel model,
            IGameHubProxy gameHubProxy,
            SignalBus signalBus, 
            IGameModuleService gameModuleService, 
            IInputManager inputManager) 
            : base(model, signalBus, gameModuleService, gameHubProxy, inputManager)
        {
        }

        /// <summary>
        /// Properties
        /// </summary>
        #region Properties

        public int Value
        {
            get => Model.Value;
            set => Model.Value = value;
        }

        #endregion
        
        /// <summary>
        /// Methods
        /// </summary>
        #region Methods
        
        public override IBaseGameModule Clone()
        {
            return null;
        }
        
        #endregion
    }
}