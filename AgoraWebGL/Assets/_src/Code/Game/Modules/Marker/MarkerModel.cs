using System.Collections.Generic;
using Agora.Core.Enums;
using UnityEngine;

namespace _src.Code.Game.Modules.Marker
{
    public class MarkerModel : GameModuleModel
    {
        [Header("Marker")]
        [SerializeField] private List<MeshRenderer> meshRenderers;
        
        private Material _material;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public MarkerModel()
        {
            Type = GameModuleType.Marker;
        }

        /// <summary>
        /// View Properties
        /// </summary>
        
        public List<MeshRenderer> MeshRenderers => meshRenderers;
        
        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public Material Material
        {
            get => _material;
            set
            {
                if (_material != value)
                {
                    _material = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}