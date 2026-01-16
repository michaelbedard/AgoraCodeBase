using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Dice;
using _src.Code.Game.Modules.Token;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderDiceFactory : PlaceholderFactory<DiceDto, IDice>
    {
    }
    
    public class DiceFactory : GameModuleFactory<Dice, DiceModel, DiceDto, IDice>, IDiceFactory
    {
        public DiceFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }

        protected override async Task<IDice> Setup(Dice dice, DiceDto loadData)
        {
            dice = (Dice)await base.Setup(dice, loadData);
            
            dice.CanBeDrag = false;
            
            return dice;
        }
    }
}