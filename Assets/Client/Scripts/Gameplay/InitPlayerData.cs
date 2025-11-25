using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Player")]
    public class InitPlayerData : ScriptableObject
    {
        public Platform platformPrefab = default;
        public Ball startBallPrefab = default;

        [Min(1)]
        public int lifes = 3;
        [Min(0)]
        public int startScores = 0;
        public int startLevel = 0;
    }
}