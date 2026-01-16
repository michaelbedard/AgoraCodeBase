using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using Zenject;

namespace _src.Code.App.Services
{
    public class ParticleSystemService : IParticleSystemService
    {
        private readonly ICameraPlaneService _cameraPlaneService;
        private readonly ParticleSystem _particleSystem;

        [Inject]
        public ParticleSystemService(ICameraPlaneService cameraPlaneService, ParticleSystem particleSystem)
        {
            _cameraPlaneService = cameraPlaneService;
            _particleSystem = particleSystem;
        }

        public void Play(Vector2 position)
        {
            _cameraPlaneService.PositionElement(_particleSystem.transform, position, normalRotation: 90f);
            _particleSystem.Play();
        }
        
        public void Play(Transform positionTransform)
        {
            var position = _cameraPlaneService.GetTransformPlaneCoordinate(positionTransform.position);
            
            Debug.LogWarning(position.x);
            Debug.LogWarning(position.y);
            
            _cameraPlaneService.PositionElement(_particleSystem.transform, position, normalRotation: 90f);
            _particleSystem.Play();
        }
    }
}