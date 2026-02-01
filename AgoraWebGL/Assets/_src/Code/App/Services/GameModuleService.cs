using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;
using JetBrains.Annotations;
using Zenject;

namespace _src.Code.App.Services
{
    public class GameModuleService : IGameModuleService
    {
        // dictionaries
        private readonly Dictionary<string, IBaseGameModule> _idToGameModule;
        private readonly Dictionary<InputObject, IBaseGameModule> _inputObjectToGameModule;
        
        // factories
        private readonly ICardFactory _cardFactory;
        private readonly ICounterFactory _counterFactory;
        private readonly IDeckFactory _deckFactory;
        private readonly ITokenFactory _iTokenFactory;
        private readonly IZoneFactory _zoneFactory;
        private readonly ITokenFactory _tokenFactory;
        private readonly IDiceFactory _diceFactory;
        private readonly IMarkerFactory _markerFactory;

        /// <summary>
        /// Constructor
        /// </summary>
        [Inject]
        public GameModuleService(
            ICardFactory cardFactory, 
            IDeckFactory deckFactory,
            IZoneFactory zoneFactory)
        {
            _cardFactory = cardFactory;
            _deckFactory = deckFactory;
            _zoneFactory = zoneFactory;
            
            _idToGameModule = new Dictionary<string, IBaseGameModule>();
            _inputObjectToGameModule = new Dictionary<InputObject, IBaseGameModule>();
        }
        
        /// <summary>
        /// GetGameModuleById
        /// </summary>
        public IBaseGameModule GetGameModuleById(string id)
        {
            return _idToGameModule[id];
        }
        
        /// <summary>
        /// GetGameModuleById
        /// </summary>
        public T GetGameModuleById<T>(string id) where T : IBaseGameModule
        {
            if (string.IsNullOrEmpty(id) || _idToGameModule[id] is not T)
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            return (T)_idToGameModule[id];
        }

        /// <summary>
        /// GetGameModuleByCollider
        /// </summary>
        public IBaseGameModule GetGameModuleByInputObject(InputObject inputObject)
        {
            return _inputObjectToGameModule.GetValueOrDefault(inputObject);
        }

        /// <summary>
        /// GetAllGameModules
        /// </summary>
        public List<IBaseGameModule> GetAllGameModules()
        {
            return _inputObjectToGameModule.Values.ToList();
        }

        public List<T> GetAllGameModules<T>() where T : IBaseGameModule
        {
            return GetAllGameModules().Where(gm => gm is T).Cast<T>().ToList();
        }

        /// <summary>
        /// DeleteGameModule
        /// </summary>
        public void DeleteGameModule(string id)
        {
            var gameModule = _idToGameModule[id];

            _idToGameModule.Remove(gameModule.Id);
            _inputObjectToGameModule.Remove(gameModule.InputObject);
        }
        
        /// <summary>
        /// InstantiateGameModule
        /// </summary>
        public async Task<IBaseGameModule> InstantiateGameModuleAsync(GameModuleDto loadData, GameModuleType moduleType)
        {
            return moduleType switch
            {
                GameModuleType.Card => await InstantiateGameModuleAsync<ICard>(loadData),
                GameModuleType.Counter => await InstantiateGameModuleAsync<ICounter>(loadData),
                GameModuleType.Deck => await InstantiateGameModuleAsync<IDeck>(loadData),
                GameModuleType.Zone => await InstantiateGameModuleAsync<IZone>(loadData),
                GameModuleType.Token => await InstantiateGameModuleAsync<IToken>(loadData),
                GameModuleType.Dice => await InstantiateGameModuleAsync<IDice>(loadData),
                GameModuleType.Marker => await InstantiateGameModuleAsync<IMarker>(loadData),
                _ => throw new Exception($"Invalid moduleType {moduleType}"),
            };
        }

        /// <summary>
        /// InstantiateGameModule
        /// </summary>
        public async Task<TIModule> InstantiateGameModuleAsync<TIModule>([CanBeNull] GameModuleDto loadData) 
            where TIModule : IBaseGameModule
        {
            IBaseGameModule IBaseGameModule;
            
            if (typeof(TIModule) == typeof(ICard))
                IBaseGameModule = await _cardFactory.CreateAsync((CardDto)loadData);
            else if (typeof(TIModule) == typeof(ICounter))
                IBaseGameModule = await _counterFactory.CreateAsync((CounterDto)loadData);
            else if (typeof(TIModule) == typeof(IDeck))
                IBaseGameModule = await _deckFactory.CreateAsync((DeckDto)loadData);
            else if (typeof(TIModule) == typeof(IZone))
                IBaseGameModule = await _zoneFactory.CreateAsync((ZoneDto)loadData);
            else if (typeof(TIModule) == typeof(IToken))
                IBaseGameModule = await _tokenFactory.CreateAsync((TokenDto)loadData);
            else if (typeof(TIModule) == typeof(IDice))
                IBaseGameModule = await _diceFactory.CreateAsync((DiceDto)loadData);
            else if (typeof(TIModule) == typeof(IMarker))
                IBaseGameModule = await _markerFactory.CreateAsync((MarkerDto)loadData);
            else
            {
                throw new Exception("Not a valid type");
            }

            // bind if it has an Id
            if (loadData != null)
            {
                _idToGameModule[IBaseGameModule.Id] = IBaseGameModule;
                _inputObjectToGameModule[IBaseGameModule.InputObject] = IBaseGameModule;
            }
            
            return (TIModule)IBaseGameModule;
        }

        public TIModule InstantiateGameModule<TIModule>() where TIModule : IBaseGameModule
        { 
            if (typeof(TIModule) == typeof(ICard))
                return (TIModule)_cardFactory.Create();
            else if (typeof(TIModule) == typeof(ICounter))
                return (TIModule)_counterFactory.Create();
            else if (typeof(TIModule) == typeof(IDeck))
                return (TIModule)_deckFactory.Create();
            else if (typeof(TIModule) == typeof(IZone))
                return (TIModule)_zoneFactory.Create();
            else if (typeof(TIModule) == typeof(IToken))
                return (TIModule)_tokenFactory.Create();
            else if (typeof(TIModule) == typeof(IDice))
                return (TIModule)_diceFactory.Create();
            else if (typeof(TIModule) == typeof(IMarker))
                return (TIModule)_markerFactory.Create();
            else
            {
                throw new Exception("Not a valid type");
            }
        }
    }
}