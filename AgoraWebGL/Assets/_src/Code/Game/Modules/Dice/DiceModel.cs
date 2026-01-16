using Agora.Core.Enums;
using UnityEngine;

namespace _src.Code.Game.Modules.Dice
{
    public class DiceModel : GameModuleModel
    {
        [Header("Dice")]
        [SerializeField] private GameObject diceObject;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public DiceModel()
        {
            Type = GameModuleType.Dice;
        }

        /// <summary>
        /// View Properties
        /// </summary>
        
        public GameObject DiceObject => diceObject;
    }
}