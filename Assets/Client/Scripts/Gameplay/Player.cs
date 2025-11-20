using System.Collections.Generic;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class Player : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body = default;

        [SerializeField]
        private Transform ballsSpawnPoint = default;
        [SerializeField]
        private Vector2 launchDirection = Vector2.up;
        [SerializeField]
        private float launchAngle = 60;
        [SerializeField]
        private int startBalls = 1;
        private List<Ball> ballsForLaunch;

        [SerializeField]
        private float maxSpeed = 5f;
        [SerializeField]
        private float stoppongDistance = 0.2f;

        [SerializeField]
        private float speedBonus = 0.1f;


        public void LaunchBalls()
        {
            for(int i = 0; i < ballsForLaunch.Count; i++)
            {
                Ball ball = ballsForLaunch[i];
                ball.transform.SetParent(default);
                ball.Body.simulated = true;
                ball.Speed = ball.startSpeed;
                ball.Body.velocity = GetRandomDirection() * ball.startSpeed;
            }

            ballsForLaunch.Clear();
        }

        public ICollection<Ball> SpawnBalls(Ball ballPrefab)
        {
            if(ballsForLaunch == default)
            {
                ballsForLaunch = new List<Ball>();
            }

            for(int i = 0; i < startBalls; i++)
            {
                Ball ball = Instantiate(ballPrefab, ballsSpawnPoint.position, ballPrefab.transform.rotation, ballsSpawnPoint);
                ball.Body.simulated = false;
                ballsForLaunch.Add(ball);
            }

            return ballsForLaunch;
        }

        public void MoveTo(in Vector2 position)
        {
            float x = position.x;
            float distance = x - transform.position.x;
            if(Mathf.Abs(distance) < stoppongDistance)
            {
                body.velocity = Vector2.zero;
            }
            else
            {
                float direction = distance < 0f ? -1f : 1f;
                body.velocity = new Vector2(direction * maxSpeed, 0f);
            }
        }

        private Vector2 GetRandomDirection()
        {
            var angle = Random.Range(-launchAngle, launchAngle) * 0.5f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 direction = rot * launchDirection;
            return direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent(out Ball ball))
            {
                ball.Speed += speedBonus;
                

                var direction = (ball.transform.position-transform.position).normalized;
                ball.Direction = direction;

            }
        }

        public Rigidbody2D Body
        {
            get => body;
        }

        public Transform BallsSpawnPoint
        {
            get => ballsSpawnPoint;
        }

        public float MaxSpeed
        {
            get => maxSpeed;
            set
            {
                maxSpeed = value;
                Vector2 direction = body.velocity.normalized;
                body.velocity = direction * maxSpeed;
            }
        }


        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;

            Vector3 origin = ballsSpawnPoint.position;
            Vector3 dir = new Vector3(launchDirection.x, launchDirection.y, 0).normalized;
            float halfAngle = launchAngle * 0.5f;
            float length = 3f;

            Quaternion leftRot = Quaternion.AngleAxis(-halfAngle, Vector3.forward);
            Quaternion rightRot = Quaternion.AngleAxis(halfAngle, Vector3.forward);

            Vector3 leftDir = leftRot * dir;
            Vector3 rightDir = rightRot * dir;

            Gizmos.color = Color.green;
            Gizmos.DrawLine(origin, origin + dir * length);

            Gizmos.color = Color.red;
            Gizmos.DrawLine(origin, origin + leftDir * length);
            Gizmos.DrawLine(origin, origin + rightDir * length);
        }

    }
}