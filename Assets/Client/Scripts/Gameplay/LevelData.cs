using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace miniIT.Arcanoid
{
    public class LevelData : MonoBehaviour
    {
        public List<Brick> bricks = default;
        public Vector2 BrickSize;

        [SerializeField]
        private KillZone killZone = default;

        [SerializeField]
        private Transform spawnPoint = default;

        [SerializeField]
        private Camera mainCamera = default;

        [SerializeField]
        private LevelValues levelValues = default;

        [Button]
        public void CollectBricks()
        {
            bricks.Clear();
            bricks.AddRange(FindObjectsByType<Brick>(FindObjectsInactive.Include, FindObjectsSortMode.None));
        }

        [Button]
        public void Snap()
        {
            for(int i = 0; i < bricks.Count; i++)
            {
                Vector2 position = bricks[i].transform.position;
                position.x = Mathf.RoundToInt(position.x / BrickSize.x) * BrickSize.x;
                position.y = Mathf.RoundToInt(position.y / BrickSize.y) * BrickSize.y;
                bricks[i].transform.position = position;
            }
        }

        public KillZone KillZone
        {
            get => killZone;
        }

        public Transform SpawnPoint
        {
            get => spawnPoint;
        }

        public Camera MainCamera => mainCamera;
        public LevelValues LevelValues => levelValues;
    }
}