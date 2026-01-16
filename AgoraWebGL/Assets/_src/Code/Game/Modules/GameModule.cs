using System;
using System.Collections.Generic;
using System.ComponentModel;
using _src.Code.Core.Actors;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Interfaces.GameModules.Other;
using _src.Code.Core.Interfaces.Managers;
using _src.Code.Core.Interfaces.Proxies;
using _src.Code.Core.Interfaces.Services;
using _src.Code.Core.Utility.MVC;
using Agora.Core.Dtos.Game.GameModules;
using Agora.Core.Enums;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Modules
{
    public abstract partial class GameModule<TModel> : BaseController<TModel>, IBaseGameModule
        where TModel : GameModuleModel
    {
        protected SignalBus SignalBus { get; }
        protected IGameModuleService GameModuleService { get; }
        protected IGameHubProxy GameHubProxy { get; }
        protected IInputManager InputManager { get; }

        
        /// <summary>
        /// Constructor
        /// </summary>
        protected GameModule(TModel model, SignalBus signalBus, IGameModuleService gameModuleService, IGameHubProxy gameHubProxy, IInputManager inputManager)
        : base(model)
        {
            SignalBus = signalBus;
            GameModuleService = gameModuleService;
            GameHubProxy = gameHubProxy;
            InputManager = inputManager;

            CanBeDrag = true;
            Initialize();
        }

        public abstract IBaseGameModule Clone();
        
        /// <summary>
        /// Listeners
        /// </summary>

        protected override void OnModelPropertyChanged(object sender, PropertyChangedEventArgs args) {
            switch (args.PropertyName)
            {
                // case nameof(XXX):
                //     // do smt
                //     break;
            }
        }
        
        protected override void SubscribeToSignals()
        {
            // manage actions
            Model.OnClick += DoClickAction;
            Model.OnDragStart += OnDragStart;
            Model.OnDragUpdate += OnDragUpdate;
            Model.OnDragEnd += (l, v) => OnDragEnd(l);
        }

        /// <summary>
        /// Properties
        /// </summary>

        #region Properties
        
        public string Id => Model.Id;
        public string Name => Model.Name;

        public GameModuleType Type
        {
            get => Model.Type;
            set => Model.Type = value;
        }
        
        public string Description
        {
            get => Model.Description;
            set => Model.Description = value;
        }

        public bool IsDropTarget
        {
            get => Model.IsDropTarget;
            set => Model.IsDropTarget = value;
        }
        
        public List<ClickAction> ClickActions
        {
            get => Model.ClickActions;
            set => Model.ClickActions = value;
        }

        public List<DragAction> DragActions
        {
            get => Model.DragActions;
            set => Model.DragActions = value;
        }

        #endregion
    }
}