using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface IParticleSystemService
    {
        void Play(Vector2 position);
        void Play(Transform transform);
    }
}