using TriInspector;
using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Player")]
    public class InitPlayerData : ScriptableObject
    {
        public Platform platformPrefab = default;
        public Ball startBallPrefab = default;

        public int lifes = 3;
        public int startScores = 0;
        public int startLevel = 0;
    }
}