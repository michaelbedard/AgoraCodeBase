using System.Collections.Generic;
using _src.Code.Core.Actors;
using _src.Code.Core.Utility.MVC;
using Agora.Core.Enums;
using UnityEngine;

namespace _src.Code.Core.Interfaces.GameModules.Other
{
    public interface IBaseGameModule : IBaseController
    {
        string Id { get; }
        string Name { get; }
        GameModuleType Type { get; set; }
        string Description { get; set; }
        bool IsDropTarget { get; set; }
        List<ClickAction> ClickActions { get; set; }
        List<DragAction> DragActions { get; set; }
        
        // Methods
        void SetParent(Transform transform);
        void ResetActions();
        void Destroy();
        public void DisableCollider();
        public void EnableCollider();
        public void EnableGlow();
        public void DisableGlow();
        public void UpdateGlowColor();
        public void ResetInputEvents();
        
        // Setup
        public IBaseGameModule Clone();
    }
}