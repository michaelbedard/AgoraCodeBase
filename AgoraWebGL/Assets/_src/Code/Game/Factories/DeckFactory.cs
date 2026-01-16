using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Deck;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderDeckFactory : PlaceholderFactory<DeckDto, IDeck>
    {
    }

    public class DeckFactory : GameModuleFactory<Deck, DeckModel, DeckDto, IDeck>, IDeckFactory
    {
        public DeckFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }
        
        protected override async Task<IDeck> Setup(Deck deck, DeckDto loadData)
        {
            deck = (Deck)await base.Setup(deck, loadData);
            
            deck.Model.BackImage.sprite = await Addressables.LoadAssetAsync<Sprite>(loadData.TopImage).Task;
            
            if (ColorUtility.TryParseHtmlString(loadData.Color, out var newColor))
            {
                deck.Model.Material = new Material(deck.Model.MeshRenderer.material);
                deck.Model.MeshRenderer.material = deck.Model.Material;
                
                deck.Model.Material.color = newColor;
            }

            deck.CanBeDrag = false;

            return deck;
        }
    }
}