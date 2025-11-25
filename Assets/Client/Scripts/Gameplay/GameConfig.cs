using System.Collections.Generic;
using TriInspector;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {
        [Scene]
        public string menuSceneName;

        [Range(0f, 1f)]
        public float defaultSFXVolume = 0.5f;
        
        [Range(0f, 1f)]
        public float defaultMusicvolume = 0.5f;

        public List<AssetReference> levels = default;
    }
}