using System.Collections.ObjectModel;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _src.Code.Game.Modules.Deck
{
    public class Deck : GameModule<DeckModel>, IDeck
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Deck(
            DeckModel model,
            SignalBus signalBus, 
            IGameModuleService gameModuleService, 
            IGameHubProxy gameHubProxy,
            IInputManager inputManager) 
            : base(model, signalBus, gameModuleService, gameHubProxy, inputManager)
        {
            Type = GameModuleType.Deck;
        }
        
        /// <summary>
        /// Properties
        /// </summary>
        #region Properties

        public ObservableCollection<ICard> Cards
        {
            get => Model.Cards;
            set => Model.Cards = value;
        }
        
        #endregion

        /// <summary>
        /// Methods
        /// </summary>
        #region Methods

        public override IBaseGameModule Clone()
        {
            var deckClone = (Deck)GameModuleService.InstantiateGameModule<IDeck>();
            
            deckClone.Model.BackImage.sprite = Model.BackImage.sprite;
            deckClone.Model.Material.color = Model.Material.color;
            
            deckClone.ResetInputEvents();
            
            return deckClone;
        }
        
        public Transform GetTopDeckTransform()
        {
            return Model.TopDeckTransform;
        }
        
        public void Bounce()
        {
            if (Model.IsBouncing) return;

            Model.IsBouncing = true;
            Transform.DOJump(Transform.position, 0.5f, 1, 0.3f).SetEase(Ease.OutBounce).OnComplete(() =>
            {
                Model.IsBouncing = false;
            });
        }
        
        public void MoveToTheRight(float amount)
        {
            Transform.position = Transform.position + new Vector3(amount * 2, 0, 0);
        }

        #endregion
    }
}