using System.Collections.Generic;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Managers
{
    public interface IInputManager
    {
        List<InputObject> InputObjectsAtMousePosition { get; }
        bool MouseOverUI { get; set; }
        bool IsDragging { get; }
        void EnableGamePlay();
        void DisableGamePlay();
    }
}