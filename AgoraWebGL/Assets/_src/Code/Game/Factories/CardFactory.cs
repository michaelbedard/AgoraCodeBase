using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Card;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderCardFactory : PlaceholderFactory<CardDto, ICard>
    {
    }
    
    public class CardFactory : GameModuleFactory<Card, CardModel, CardDto, ICard>, ICardFactory
    {
        [Inject]
        public CardFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }

        protected override async Task<ICard> Setup(Card card, CardDto loadData)
        {
            card = (Card)await base.Setup(card, loadData);
            
            card.Model.FrontImage.sprite = await Addressables.LoadAssetAsync<Sprite>(loadData.FrontImage).Task;
            
            if (!string.IsNullOrWhiteSpace(loadData.BackImage))
                card.Model.BackImage.sprite = await Addressables.LoadAssetAsync<Sprite>(loadData.BackImage).Task;
            
            card.CanBeClick = false;

            return card;
        }
    }
    
}