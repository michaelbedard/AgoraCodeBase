using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;
using Exception = System.Exception;

namespace _src.Code.Core.Utility.MVC
{
    public abstract class BaseFactory<TModule, TModel, TProps, TIModule> : IFactory<TProps, TIModule>
        where TModule : BaseController<TModel>, TIModule
        where TModel : BaseModel
        where TProps : class, new()
    {
        private readonly DiContainer _container;
        private readonly GameObject _prefab;

        protected BaseFactory(DiContainer container, GameObject prefab)
        {
            _container = container;
            _prefab = prefab;
        }

        protected abstract Task<TIModule> Setup(TModule obj, TProps props);

        public async Task<TIModule> CreateAsync([CanBeNull] TProps props)
        {
            var obj = (TModule)Create(props);

            if (props != null)
            {
                try
                {
                    return await Setup(obj, props);
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                }
            }

            return obj;
        }
        
        public TIModule Create()
        {
            return Create(null);
        }

        public virtual TIModule Create([CanBeNull] TProps props)
        {
            var instance = GameObject.Instantiate(_prefab);
            
            var model = instance.GetComponentInChildren<TModel>();

            try
            {
                var module = _container.InstantiateExplicit<TModule>(
                    new List<TypeValuePair>
                    {
                        // new(typeof(TProps), props),
                        new(typeof(TModel), model)
                    });
                
                return module;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                throw new Exception($"Failed to create instance of {typeof(TModule).FullName}", e);
            }
        }
    }
}