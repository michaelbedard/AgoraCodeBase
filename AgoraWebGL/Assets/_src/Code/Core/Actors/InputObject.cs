using System;
using System.Collections.Generic;
using UnityEngine;

namespace _src.Code.Core.Actors
{
    public class InputObject : MonoBehaviour
    {
        [Header("Inputs")]
        [SerializeField] 
        private List<Collider> colliders;

        /// <summary>
        /// Actions
        /// </summary>
        public Action OnEnter;
        public Action OnExit;
        public Action OnClick;
        public Action<Vector2> OnDragStart;
        public Action<InputObject, Vector2> OnDragUpdate;
        public Action<List<InputObject>, Vector2> OnDragEnd;
        public Action OnHoverStart;
        public Action OnHoverEnd;
        public Action<Vector2> OnScroll;
        
        /// <summary>
        /// Mono Properties
        /// </summary>
        public List<Collider> Colliders => colliders;

        /// <summary>
        /// Object Properties
        /// </summary>
        
        public bool CanBeClick { get; set; } = true;
        public bool CanBeDrag { get; set; } = true;
        public bool CanBeHover { get; set; } = true;
        public bool CanBeScroll { get; set; } = true;
        
        public bool IsBeingDrag { get; set; } = false;
    }
}