using TriInspector;
using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/Level values")]
    public class LevelValues : ScriptableObject
    {
        [Header("Balls speed")]
        public float minBallsSpeed = 2f;
        public float maxBallsSpeed = 20f;
        [Slider(nameof(minBallsSpeed), nameof(maxBallsSpeed))]
        public float startBallsSpeed = 4f;

        [Header("Balls size")]
        public float minBallsSize = 0.3f;
        public float maxBallsSize = 3f;
        [Slider(nameof(minBallsSize), nameof(maxBallsSize))]
        public float startBallsSize =1f;

        [Header("Platform size")]
        public float minPlatformSize = 0.5f;
        public float maxPlatformSize = 3f;
        [Slider(nameof(minPlatformSize), nameof(maxPlatformSize))]
        public float startPlatformSize = 1f;
    }
}