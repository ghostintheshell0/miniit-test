using TriInspector;
using UnityEngine;

namespace miniit.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Player")]
    public class InitPlayerData : ScriptableObject
    {
        public Platform platformPrefab = default;
        public Ball startBallPrefab = default;

        public int lifes = 3;
        public int startScores = 0;

        [Header("Balls speed")]
        public float minBallsSpeed = 2f;
        public float maxBallsSpeed = 20f;
        [Slider(nameof(minBallsSpeed), nameof(maxBallsSpeed))]
        public float startBallsSpeed = 4f;

        [Header("Balls size")]
        public float minBallsSize = 0.3f;
        public float maxBallsSize = 3f;
        [Slider(nameof(minBallsSize), nameof(maxBallsSize))]
        public float startBallsSize;

        [Header("Platform size")]
        public float minPlatformSize = 0.5f;
        public float maxPlatformSize = 3f;
        [Slider(nameof(minPlatformSize), nameof(maxPlatformSize))]
        public float startPlatformSize = 1f;
    }
}