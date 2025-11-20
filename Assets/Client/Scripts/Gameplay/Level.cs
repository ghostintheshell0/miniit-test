using System.Collections.Generic;
using TriInspector;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class Level : MonoBehaviour
    {
        public List<Brick> bricks;
        public Vector2 BrickSize;

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
    }
}