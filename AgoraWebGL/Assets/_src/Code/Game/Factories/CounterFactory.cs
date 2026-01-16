using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Counter;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderCounterFactory : PlaceholderFactory<CounterDto, ICounter>
    {
    }
    
    public class CounterFactory : GameModuleFactory<Counter, CounterModel, CounterDto, ICounter>, ICounterFactory
    {
        public CounterFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }
        
        protected override async Task<ICounter> Setup(Counter counter, CounterDto loadData)
        {
            counter = (Counter)await base.Setup(counter, loadData);
            
            // if (data.TryGetValue("Color", out var colorObject))
            // {
            //     var colorHtmlString = (string)colorObject;
            //     
            //     if (ColorUtility.TryParseHtmlString(colorHtmlString, out var newColor))
            //     {
            //         image.color = newColor;
            //     }
            //     else
            //     {
            //         Debug.LogError("Invalid color string: " + colorHtmlString);
            //     }
            // }


            return null;
        }
    }
}