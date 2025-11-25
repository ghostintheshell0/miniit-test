using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class BallsMultiplerBonus : BaseBonus
    {
        [Min(2)]
        public int count = 2;

        public float sizeMultipler = 0.5f;

        private LevelController levelController = default;
        
        [Inject]
        public override void Inject(IObjectResolver resolver)
        {
            base.Inject(resolver);
            levelController = resolver.Resolve<LevelController>();
        }

        protected override void Apply(Platform platform)
        {
            for(int i = levelController.Balls.Count - 1; i >= 0; i--)
            {
                Ball prefab = levelController.Balls[i];
                Vector2 direction = prefab.Direction;
                float angleDelta = 360f / count * Mathf.Deg2Rad;
                float startAngle = Mathf.Atan2(direction.y, direction.x);
                
                for(int k = 1; k < count; k++)
                {
                    Ball newBall = resolver.Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
                    float angleRad = Random.Range(-180f, 180f) * Mathf.Deg2Rad;
                    Vector3 newDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
                    levelController.Balls.Add(newBall);
                    newBall.Speed = levelController.BallsSpeed;
                    newBall.Direction = newDirection;
                }
            }

            levelController.BallsSize *= sizeMultipler;
        }
    }
}