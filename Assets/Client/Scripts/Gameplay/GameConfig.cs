using TriInspector;
using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Config")]
    public class GameConfig : ScriptableObject
    {
        [Scene]
        public string menuSceneName;
    }
}