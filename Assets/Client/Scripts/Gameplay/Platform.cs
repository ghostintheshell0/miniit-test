using System.Collections.Generic;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class Platform : MonoBehaviour
    {
        public Player player;

        [SerializeField]
        private Rigidbody2D body = default;

        [Header("launch balls")]

        [SerializeField]
        private Transform ballsSpawnPoint = default;
        [SerializeField]
        private Vector2 launchDirection = Vector2.up;
        [SerializeField]
        private float launchAngle = 60;
        [SerializeField]
        private List<Ball> connectedBalls = default;

        [Header("Move")]
        [SerializeField]
        private float maxSpeed = 5f;
        [SerializeField]
        private float stoppongDistance = 0.2f;

        [SerializeField]
        private float speedBonus = 0.1f;

        [Header("Size")]
        [SerializeField]
        private float minSize = 0.5f;
        [SerializeField]
        private float maxSize = 5f;
        [SerializeField]
        private float size = 1f;


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

        public void LaunchBalls()
        {
            for(int i = 0; i < connectedBalls.Count; i++)
            {
                Ball ball = connectedBalls[i];
                ball.transform.SetParent(default);
                ball.Simulated = true;
                ball.Speed = ball.startSpeed;
                ball.Body.velocity = GetRandomDirection() * ball.startSpeed;
            }

            connectedBalls.Clear();
        }

        public Ball SpawnBall(Ball ballPrefab)
        {
            if(connectedBalls == default)
            {
                connectedBalls = new List<Ball>();
            }
            Vector3 position = ballsSpawnPoint.position - ballPrefab.Pivot.position;
            Ball ball = Instantiate(ballPrefab, position, ballPrefab.transform.rotation, ballsSpawnPoint);
            ball.Simulated = false;
            connectedBalls.Add(ball);

            return ball;
        }


        private Vector2 GetRandomDirection()
        {
            var angle = UnityEngine.Random.Range(-launchAngle, launchAngle) * 0.5f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 direction = rot * launchDirection;
            return direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent(out Ball ball))
            {
                var direction = (ball.transform.position-transform.position).normalized;
                ball.Direction = direction;

            }
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

        public float Size
        {
            get => size;
            set
            {
                size = Mathf.Clamp(value, minSize, maxSize);
                Vector3 localScale = transform.localScale;
                localScale.x = size;
                transform.localScale = localScale;
            }
        }

        public Rigidbody2D Body
        {
            get => body;
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