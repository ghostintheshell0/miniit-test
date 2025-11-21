using UnityEngine;

namespace miniit.Arcanoid
{
    public class BallsMultiplerBonus : BaseBonus
    {
        [Min(2)]
        public int count = 2;

        public float sizeMultipler = 0.5f;

        protected override void Apply(Platform platform)
        {
            for(int i = platform.player.Balls.Count - 1; i >= 0; i--)
            {
                Ball prefab = platform.player.Balls[i];
                Vector2 direction = prefab.Direction;
                float angleDelta = 360f / count * Mathf.Deg2Rad;
                float startAngle = Mathf.Atan2(direction.y, direction.x);
                
                for(int k = 1; k < count; k++)
                {
                    Ball newBall = Instantiate(prefab, prefab.transform.position, prefab.transform.rotation);
                    float angleRad = startAngle + angleDelta * k;
                    Vector3 newDirection = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
                    platform.player.Balls.Add(newBall);
                    newBall.Speed = platform.player.BallsSpeed;
                    newBall.Direction = newDirection;
                }
            }

            platform.player.BallsSize *= sizeMultipler;
        }
    }
}