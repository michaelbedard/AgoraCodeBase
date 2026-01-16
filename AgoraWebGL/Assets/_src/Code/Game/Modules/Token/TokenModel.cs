using Agora.Core.Enums;
using TMPro;
using UnityEngine;

namespace _src.Code.Game.Modules.Token
{
    public class TokenModel : GameModuleModel
    {
        [SerializeField] 
        private MeshRenderer meshRenderer;
        [SerializeField] 
        public Material material;
        [SerializeField] 
        public TextMeshProUGUI text;

        private Color _color;
        
        /// <summary>
        /// Constructor
        /// </summary>
        public TokenModel()
        {
            Type = GameModuleType.Token;

            if (meshRenderer != null)
            {
                // Create a unique instance of the material for this object
                material = new Material(meshRenderer.material);
                meshRenderer.material = material;
            }
        }
        
        /// <summary>
        /// View Properties
        /// </summary>
        
        public TextMeshProUGUI Text => text;
        public Material Material => material;
        
        /// <summary>
        /// Controller Properties
        /// </summary>
        
        public Color Color
        {
            get => _color;
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}