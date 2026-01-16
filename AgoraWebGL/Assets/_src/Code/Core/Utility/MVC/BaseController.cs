using System;
using System.Collections.Generic;
using System.ComponentModel;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using UnityEngine;

namespace _src.Code.Core.Utility.MVC
{
    public class BaseController<TModel> : IBaseController
        where TModel : BaseModel
    {
        public TModel Model { get; }

        protected BaseController(TModel model)
        {
            Model = model;
        }
        
        public void ResetInputEvents()
        {
            Model.OnClick = null;
            Model.OnDragStart = null;
            Model.OnDragUpdate = null;
            Model.OnDragEnd = null;
            Model.OnHoverStart = null;
            Model.OnHoverEnd = null;
            Model.OnScroll = null;
        }
        
        /// <summary>
        /// Initialize and Subscribe
        /// </summary>
        
        protected void Initialize()
        {
            try
            {
                // Subscribe
                Model.PropertyChanged += OnModelPropertyChanged;
                SubscribeToSignals();
            }
            catch (Exception e)
            {
                Debug.LogError($"{e.Message}\n{e.StackTrace}");
            }
        }

        protected virtual void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args) 
        {
            // empty
        }
        
        protected virtual void SubscribeToSignals()
        {
            // empty
        }

        
        public Transform Transform => Model.Transform;
        public List<Collider> Colliders => Model.Colliders;
        public InputObject InputObject => Model;
        
        public bool CanBeClick
        {
            get => Model.CanBeClick;
            set => Model.CanBeClick = value;
        }

        public bool CanBeDrag
        {
            get => Model.CanBeDrag;
            set => Model.CanBeDrag = value;
        }
        
        public bool CanBeHover
        {
            get => Model.CanBeHover;
            set => Model.CanBeHover = value;
        }

        public bool CanBeScroll
        {
            get => Model.CanBeScroll;
            set => Model.CanBeScroll = value;
        }
    }
}