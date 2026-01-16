using System.Collections.ObjectModel;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules.Other;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules
{
    public interface IDeck : IBaseGameModule
    {
        // Properties
        ObservableCollection<ICard> Cards { get; set; }
        
        // Methods
        Transform GetTopDeckTransform();
        void Bounce();
        void MoveToTheRight(float amount);
    }
}