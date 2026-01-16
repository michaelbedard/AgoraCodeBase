using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Core.Utility.MVC;
using _src.Code.Game.Modules;
using _src.Code.Game.Modules.Card;
using Agora.Core.Dtos.Game.GameModules;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Exception = System.Exception;

namespace _src.Code.Game.Factories
{
    public abstract class GameModuleFactory<TModule, TModel, TDto, TIModule> : BaseFactory<TModule, TModel, TDto, TIModule>
        where TModule : GameModule<TModel>, TIModule
        where TModel : GameModuleModel
        where TDto : GameModuleDto, new()
        where TIModule : IBaseController
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        protected GameModuleFactory(DiContainer container, GameObject prefab) : base(container, prefab)
        {
        }

        protected override async Task<TIModule> Setup(TModule obj, TDto props)
        {
            obj.Model.Id = props.Id;
            obj.Model.Name = props.Name;

            return obj;
        }
    }
}