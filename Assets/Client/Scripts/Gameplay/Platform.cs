using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class Platform : MonoBehaviour
    {

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
        private float stoppingDistance = 0.2f;


        [Header("Size")]
        [SerializeField]
        private float minSize = 0.5f;
        [SerializeField]
        private float maxSize = 5f;
        [SerializeField]
        private float size = 1f;
        [SerializeField]
        private float platformSize = 1f;

        private IObjectResolver resolver = default;
        private LevelController levelController = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
            levelController = resolver.Resolve<LevelController>();
        }
        public void MoveTo(Vector2 position)
        {
            Vector2 distance = position - body.position;
            distance.y = 0;

            if(distance.sqrMagnitude > stoppingDistance * stoppingDistance)
            {
                Vector2 dir = distance.normalized;
                Vector2 offset = new Vector2((platformSize * size * 0.5f + 0.02f) * dir.x, 0f);
                RaycastHit2D hit = Physics2D.Raycast(body.position+offset, dir, 0.02f);
                if(hit.collider == null)
                {
                    Vector2 step = dir * maxSpeed * Time.fixedDeltaTime;
                    body.MovePosition(body.position + step);
                }
                else
                {
                    body.velocity = Vector2.zero;
                }
            }
            else
            {
                body.velocity = Vector2.zero;
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
            Ball ball = resolver.Instantiate(ballPrefab, position, ballPrefab.transform.rotation, ballsSpawnPoint);
            ball.Simulated = false;
            connectedBalls.Add(ball);

            return ball;
        }


        private Vector2 GetRandomDirection()
        {
            float angle = UnityEngine.Random.Range(-launchAngle, launchAngle) * 0.5f;
            Quaternion rot = Quaternion.AngleAxis(angle, Vector3.forward);
            Vector3 direction = rot * launchDirection;
            return direction;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent(out Ball ball))
            {
                Vector3 direction = (ball.transform.position-transform.position).normalized;
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

            float offset = platformSize * size * 0.5f + 0.02f;
            Vector3 hit1 = transform.position;
            hit1.x += offset;
            Vector3 hit2 = transform.position;
            hit2.x -= offset;

            Gizmos.DrawWireSphere(hit1, 0.1f);
            Gizmos.DrawWireSphere(hit2, 0.1f);
        }
    }
}