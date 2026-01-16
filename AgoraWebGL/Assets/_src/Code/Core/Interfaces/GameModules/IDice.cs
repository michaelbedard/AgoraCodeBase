using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules.Other;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface IDice : IBaseGameModule
    {
        // Properties
        int Number { get; set; }
        GameObject DiceObject { get; }

        // Methods
    }
}