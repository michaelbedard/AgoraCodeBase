using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Marker;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderMarkerFactory : PlaceholderFactory<MarkerDto, IMarker>
    {
    }
    
    public class MarkerFactory : GameModuleFactory<Marker, MarkerModel, MarkerDto, IMarker>, IMarkerFactory
    {
        public MarkerFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }

        protected override async Task<IMarker> Setup(Marker marker, MarkerDto loadData)
        {
            marker = (Marker)await base.Setup(marker, loadData);
            
            if (ColorUtility.TryParseHtmlString(loadData.IdentifiableColor, out var newColor))
            {
                // extract material
                marker.Model.Material = new Material(marker.Model.MeshRenderers[0].material);
                marker.Model.Material.mainTexture = null;
                marker.Model.Material.color = newColor;
                
                // set to each mesh renderer
                foreach (var meshRenderer in marker.Model.MeshRenderers)
                {
                    meshRenderer.material = marker.Model.Material;
                }
            }
            
            marker.CanBeDrag = false;
            
            return marker;
        }
    }
}