using System;
using System.Collections.Generic;
using System.Linq;
using _src.Code.Core.Utility;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;

namespace _src.Settings
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configuration/UIConfig")]
    public class UIConfig : ScriptableObject
    {
        [Serializable]
        public class SpriteEntry
        {
            public string Key; // Read-only (Auto-filled)
            public Sprite SpriteAsset;
        }

        [Serializable]
        public class UIEntry
        {
            public string TypeName; 
            public AssetReferenceT<VisualTreeAsset> VisualTreeAsset;
            
            // Sprites are now nested HERE, specific to this UI element
            public List<SpriteEntry> RequiredSprites = new List<SpriteEntry>();
        }

        [SerializeField] private List<UIEntry> entries;

        // --- LOOKUP METHODS ---

        public AssetReference GetAssetReference<T>()
        {
            var typeName = typeof(T).Name;
            var entry = entries.FirstOrDefault(x => x.TypeName == typeName);
            return entry?.VisualTreeAsset;
        }

        public Sprite GetSprite<T>(string key)
        {
            var typeName = typeof(T).Name;
            var entry = entries.FirstOrDefault(x => x.TypeName == typeName);
            
            var spriteEntry = entry?.RequiredSprites.FirstOrDefault(x => x.Key == key);
            
            if (spriteEntry?.SpriteAsset == null)
            {
                Debug.LogWarning($"[UIConfig] Missing Sprite for '{typeName}' with key '{key}'");
                return null;
            }

            return spriteEntry.SpriteAsset;
        }
        
        public Sprite GetSprite(string typeName, string key)
        {
            // 1. Find the UI Entry by name (e.g. "PrimaryButton")
            var entry = entries.FirstOrDefault(x => x.TypeName == typeName);
            
            if (entry == null)
            {
                Debug.LogWarning($"[UIConfig] No UIEntry found for type '{typeName}'");
                return null;
            }

            // 2. Find the Sprite inside that entry
            var spriteEntry = entry.RequiredSprites.FirstOrDefault(x => x.Key == key);
            
            if (spriteEntry == null || spriteEntry.SpriteAsset == null)
            {
                // Optional: Log warning if sprite is missing
                Debug.LogWarning($"[UIConfig] Missing Sprite '{key}' for '{typeName}'");
                return null;
            }

            return spriteEntry.SpriteAsset;
        }

        // --- MAGIC AUTO-FILL LOGIC ---
#if UNITY_EDITOR
        private void OnValidate()
        {
            if (entries == null) return;
            
            entries.RemoveAll(x => x.VisualTreeAsset == null && string.IsNullOrEmpty(x.TypeName));

            foreach (var entry in entries)
            {
                // 1. Sync TypeName with the dragged Asset
                if (entry.VisualTreeAsset != null && entry.VisualTreeAsset.editorAsset != null)
                {
                    string assetName = entry.VisualTreeAsset.editorAsset.name;

                    // If the names don't match (e.g. copied from previous entry, or dragged new file)
                    if (entry.TypeName != assetName)
                    {
                        entry.TypeName = assetName;
                        // IMPORTANT: Clear the old sprites from the previous entry copy
                        entry.RequiredSprites.Clear(); 
                    }
                }

                // 2. Scan for Attributes if we have a valid TypeName
                if (!string.IsNullOrEmpty(entry.TypeName))
                {
                    UpdateSpriteRequirements(entry);
                }
            }
        }

        private void UpdateSpriteRequirements(UIEntry entry)
        {
            // 1. Find ALL classes with this name
            var potentialMatches = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.Name == entry.TypeName)
                .ToList();

            // 2. Sort them: User code (_src) First, everything else Second
            Type targetType = potentialMatches
                .OrderByDescending(t => t.Namespace != null && t.Namespace.StartsWith("_src"))
                .FirstOrDefault();

            if (targetType == null)
            {
                return;
            }

            // 3. Get attributes
            var attributes = targetType.GetCustomAttributes(typeof(RequiredSpriteAttribute), true)
                .Cast<RequiredSpriteAttribute>()
                .ToList();

            // 4. Update List
            if (entry.RequiredSprites == null) entry.RequiredSprites = new List<SpriteEntry>();

            // Add missing
            foreach (var attr in attributes)
            {
                if (!entry.RequiredSprites.Any(x => x.Key == attr.Key))
                {
                    entry.RequiredSprites.Add(new SpriteEntry { Key = attr.Key });
                }
            }

            // Remove old
            for (int i = entry.RequiredSprites.Count - 1; i >= 0; i--)
            {
                if (!attributes.Any(x => x.Key == entry.RequiredSprites[i].Key))
                {
                    entry.RequiredSprites.RemoveAt(i);
                }
            }
        }
#endif
    }
}