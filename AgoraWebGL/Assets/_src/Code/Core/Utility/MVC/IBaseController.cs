using System.Collections.Generic;
using _src.Code.Core.Actors;
using UnityEngine;

namespace _src.Code.Core.Utility.MVC
{
    public interface IBaseController
    {
        // Properties
        Transform Transform { get; }
        List<Collider> Colliders { get; }
        InputObject InputObject { get; }
        
        bool CanBeClick { get; set; }
        bool CanBeDrag { get; set; }
        bool CanBeHover { get; set; }
        bool CanBeScroll { get; set; }
    }
}