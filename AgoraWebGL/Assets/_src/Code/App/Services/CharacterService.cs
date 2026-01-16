using System.Collections.Generic;
using System.Threading.Tasks;
using _src.Code.Core.Interfaces.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

namespace _src.Code.App.Services
{
    public class CharacterService : ICharacterService
    {
        private static readonly Dictionary<int, string> CharacterAddressToId = new Dictionary<int, string>()
        {
            { 0, "Assets/_src/Prefabs/Characters/Witch.prefab" },
            { 1, "Assets/_src/Prefabs/Characters/Skeleton.prefab" },
            { 2, "Assets/_src/Prefabs/Characters/Ghoul.prefab" },
        };

        [Inject]
        public CharacterService()
        {
        }
        
        public async Task<GameObject> GetCharacterById(int characterId, bool active)
        {
            if (!CharacterAddressToId.TryGetValue(characterId, out var characterAddress))
            {
                Debug.LogError($"Character ID {characterId} not found in the address map.");
                return null;
            }

            try
            {
                // Load the prefab asset. 
                var prefab = await Addressables.LoadAssetAsync<GameObject>(characterAddress).Task;

                if (prefab == null)
                {
                    Debug.LogError($"Character prefab with address {characterAddress} is null.");
                    return null;
                }

                // Instantiate the prefab and return it
                prefab.SetActive(active);
                return Object.Instantiate(prefab);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to load character with ID {characterId}: {e.Message}");
                return null;
            }
        }
    }
}