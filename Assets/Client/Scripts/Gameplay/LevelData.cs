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
        private Transform minPoint = default;
        [SerializeField]
        private Transform maxPoint = default;

        [SerializeField]
        private KillZone killZone = default;

        [SerializeField]
        private Transform spawnPoint = default;

        [SerializeField]
        private Camera mainCamera = default;

        [SerializeField]
        private LevelValues levelValues = default;

        [SerializeField]
        private CameraFitter cameraFitter = default;

        [SerializeField]
        private Transform vfxLayer = default;

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

        private void OnDrawGizmos()
        {
            if(minPoint == default || maxPoint == default)
            {
                return;
            }

            Gizmos.color = Color.green;

            Vector3 p2 = new Vector3(minPoint.position.x, maxPoint.position.y);
            Vector3 p4 = new Vector3(maxPoint.position.x, minPoint.position.y);

            Gizmos.DrawLine(minPoint.position, p2);
            Gizmos.DrawLine(p2, maxPoint.position);
            Gizmos.DrawLine(maxPoint.position, p4);
            Gizmos.DrawLine(p4, minPoint.position);
        }

        public Transform MinPoint => minPoint;
        public Transform MaxPoint => maxPoint;
        public KillZone KillZone => killZone;
        public Transform SpawnPoint => spawnPoint;
        public Camera MainCamera => mainCamera;
        public LevelValues LevelValues => levelValues;
        public CameraFitter CameraFitter => cameraFitter;
        public Transform VFXLayer => vfxLayer;
    }
}