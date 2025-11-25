using TriInspector;
using UnityEngine;

namespace miniIT.Arcanoid
{
    public class CameraFitter : MonoBehaviour
    {
        [Header("World Padding")]
        public float paddingLeftWorld = 1f;
        public float paddingRightWorld = 1f;
        public float paddingTopWorld = 1f;
        public float paddingBottomWorld = 1f;

        [Header("Screen Padding")]
        public float paddingLeftScreen = 0.05f;
        public float paddingRightScreen = 0.2f;
        public float paddingTopScreen = 0.05f;
        public float paddingBottomScreen = 0.05f;

        private Transform minPoint = default;
        private Transform maxPoint = default;
        private new Camera camera = default;

        [Button]
        public void Fit(Camera camera, LevelData levelData)
        {
            this.camera = camera;
            minPoint = levelData.MinPoint;
            maxPoint = levelData.MaxPoint;
        }

        private void LateUpdate()
        {
            if(camera == default || minPoint == default || maxPoint == default)
            {
                return;
            }
            
            Vector3 min = minPoint.position;
            Vector3 max = maxPoint.position;

            min.x -= paddingLeftWorld;
            min.y -= paddingBottomWorld;
            max.x += paddingRightWorld;
            max.y += paddingTopWorld;

            float width = max.x - min.x;
            float height = max.y - min.y;

            float requiredSize = Mathf.Max(height / 2f, width / (2f * camera.aspect));

            camera.orthographic = true;
            camera.orthographicSize = requiredSize;

            float halfHeight = requiredSize;
            float halfWidth = requiredSize * camera.aspect;

            float leftOffset = halfWidth * paddingLeftScreen;
            float rightOffset = halfWidth * paddingRightScreen;
            float topOffset = halfHeight * paddingTopScreen;
            float bottomOffset = halfHeight * paddingBottomScreen;

            min.x -= leftOffset;
            min.y -= bottomOffset;
            max.x += rightOffset;
            max.y += topOffset;

            Vector3 center = (min + max) / 2f;
            width = max.x - min.x;
            height = max.y - min.y;

            requiredSize = Mathf.Max(height / 2f, width / (2f * camera.aspect));
            camera.orthographicSize = requiredSize;

            camera.transform.position = new Vector3(center.x, center.y, camera.transform.position.z);
        }
    }
}