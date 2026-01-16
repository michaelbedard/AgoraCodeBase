using System.Threading.Tasks;
using Agora.Core.Enums;
using UnityEngine;

namespace _src.Code.Core.Interfaces.Services
{
    public interface ICharacterService
    {
        Task<GameObject> GetCharacterById(int characterId, bool instantiateInWorldSpace);
    }
}