using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Signals.Game;
using _src.Code.Game.Commands;
using _src.Code.Game.Commands.GameActions;
using _src.Code.Game.Commands.Other;
using _src.Code.Game.Factories;
using _src.Code.Game.Modules.Card;
using _src.Code.Game.Modules.Counter;
using _src.Code.Game.Modules.Deck;
using _src.Code.Game.Modules.Dice;
using _src.Code.Game.Modules.Marker;
using _src.Code.Game.Modules.Token;
using _src.Code.Game.Modules.Zone;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Other
{
    public class GameInstaller : MonoInstaller
    {
        public GameObject playerPrefab;
        public GameObject cardPrefab;
        public GameObject counterPrefab;
        public GameObject deckPrefab;
        public GameObject tokenPilePrefab;
        public GameObject zonePrefab;
        public GameObject dicePrefab;
        public GameObject markerPrefab;
        
        public override void InstallBindings()
        {
            Debug.Log("GameModulesInstaller");
            
            // description manager
            Container.BindInterfacesAndSelfTo<DescriptionManager>().AsSingle().NonLazy();
            
            // card
            Container.Bind<ICard>().To<Card>().AsTransient();
            Container.Bind<ICardFactory>().To<CardFactory>().AsTransient();
            Container.BindFactory<CardDto, ICard, PlaceholderCardFactory>().FromFactory<CardFactory>();
            
            Container.BindInstance(cardPrefab).WhenInjectedInto<CardFactory>();
            
            // counter
            Container.Bind<ICounter>().To<Counter>().AsTransient();
            Container.Bind<ICounterFactory>().To<CounterFactory>().AsTransient();
            Container.BindFactory<CounterDto, ICounter, PlaceholderCounterFactory>().FromFactory<CounterFactory>();
            
            Container.BindInstance(counterPrefab).WhenInjectedInto<CounterFactory>();
            
            // deck
            Container.Bind<IDeck>().To<Deck>().AsTransient();
            Container.Bind<IDeckFactory>().To<DeckFactory>().AsTransient();
            Container.BindFactory<DeckDto, IDeck, PlaceholderDeckFactory>().FromFactory<DeckFactory>();
            
            Container.BindInstance(deckPrefab).WhenInjectedInto<DeckFactory>();
            
            // tokenPile
            Container.Bind<IToken>().To<Token>().AsTransient();
            Container.Bind<ITokenFactory>().To<TokenFactory>().AsTransient();
            Container.BindFactory<TokenDto, IToken, PlaceholderTokenFactory>().FromFactory<TokenFactory>();
            
            Container.BindInstance(tokenPilePrefab).WhenInjectedInto<TokenFactory>();
            
            // zone
            Container.Bind<IZone>().To<Zone>().AsTransient();
            Container.Bind<IZoneFactory>().To<ZoneFactory>().AsTransient();
            Container.BindFactory<ZoneDto, IZone, PlaceholderZoneFactory>().FromFactory<ZoneFactory>();
            
            Container.BindInstance(zonePrefab).WhenInjectedInto<ZoneFactory>();
            
            // dice
            Container.Bind<IDice>().To<Dice>().AsTransient();
            Container.Bind<IDiceFactory>().To<DiceFactory>().AsTransient();
            Container.BindFactory<DiceDto, IDice, PlaceholderDiceFactory>().FromFactory<DiceFactory>();
            
            Container.BindInstance(dicePrefab).WhenInjectedInto<DiceFactory>();
            
            // marker
            Container.Bind<IMarker>().To<Marker>().AsTransient();
            Container.Bind<IMarkerFactory>().To<MarkerFactory>().AsTransient();
            Container.BindFactory<MarkerDto, IMarker, PlaceholderMarkerFactory>().FromFactory<MarkerFactory>();
            
            Container.BindInstance(markerPrefab).WhenInjectedInto<MarkerFactory>();
            
            
            Container.DeclareSignal<GameActionSignal>();
            Container.DeclareSignal<GameAnimationSignal>();
            Container.DeclareSignal<GameInputSignal>();
            
            Container.Bind<DrawCard>().AsSingle().NonLazy();
            Container.Bind<FlipTopCard>().AsSingle().NonLazy();
            Container.Bind<PlayCard>().AsSingle().NonLazy();
            Container.Bind<ActivateCard>().AsSingle().NonLazy();
            Container.Bind<PlayCardInsideZone>().AsSingle().NonLazy();
            Container.Bind<TransferToken>().AsSingle().NonLazy();
            // Container.Bind<ReplacePlayerHand>().AsSingle().NonLazy();
            // Container.Bind<RotatePlayerHands>().AsSingle().NonLazy();
            // Container.Bind<StealCardFromPlayer>().AsSingle().NonLazy();
            // Container.Bind<DiscardCard>().AsSingle().NonLazy();
            // Container.Bind<SetCounterValue>().AsSingle().NonLazy();
            // Container.Bind<AddToken>().AsSingle().NonLazy();
            // Container.Bind<RemoveToken>().AsSingle().NonLazy();
            
            Container.Bind<InputListener>().AsSingle().NonLazy();
            
            Container.Bind<PlayerTurn>().AsSingle().NonLazy();
            Container.Bind<EndGame>().AsSingle().NonLazy();
            Container.Bind<ShowMessage>().AsSingle().NonLazy();
        }
    }
}