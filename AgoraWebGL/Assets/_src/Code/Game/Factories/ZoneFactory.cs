using System;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Factories;
using _src.Code.Core.Interfaces.GameModules;
using _src.Code.Game.Modules.Zone;
using Agora.Core.Dtos.Game.GameModules;
using UnityEngine;
using Zenject;

namespace _src.Code.Game.Factories
{
    public class PlaceholderZoneFactory : PlaceholderFactory<ZoneDto, IZone>
    {
    }

    public class ZoneFactory : GameModuleFactory<Zone, ZoneModel, ZoneDto, IZone>, IZoneFactory
    {
        public ZoneFactory(DiContainer container, GameObject prefab)
            : base(container, prefab)
        {
        }

        protected override async Task<IZone> Setup(Zone zone, ZoneDto loadData)
        {
            zone = (Zone)await base.Setup(zone, loadData);
            
            if (loadData.Width > 0 && loadData.Height > 0)
                UpdateSize(zone, loadData.Width, loadData.Height);

            zone.StackingMethod = loadData.StackingMethod;
            
            zone.CanBeDrag = false;

            return zone;
        }

        private void UpdateSize(IZone zone, float width, float height)
        {
            // resize collider
            if (zone.Colliders.Count != 1)
            {
                throw new Exception("Zone should only have 1 collider");
            }
            
            if (zone.Colliders[0] is BoxCollider boxCollider)
            {
                boxCollider.size = new Vector3(width, 0, height);
            }
            else
            {
                throw new Exception("Zone should have a box collider");
            }
            
            // resize canvas.  NOTE : we add +2 because the glow image/canvas is bigger than the actual zone
            var canvasWidth = (width + 2) * 100;
            var canvasHeight = (height + 2) * 100;
            
            zone.Canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(canvasWidth, canvasHeight);
        }
    }
}