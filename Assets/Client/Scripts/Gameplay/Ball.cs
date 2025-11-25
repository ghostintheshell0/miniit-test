using TMPro;
using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body = default;

        [SerializeField]
        private new CircleCollider2D collider = default;

        [SerializeField]
        private Transform pivot = default;

        [SerializeField]
        private SoundSet platformHitSound = default;

        public float startSpeed = 5f;
        public int Damage = 1;
        
        [SerializeField]
        private float minScale = 0.5f;

        [SerializeField]
        private float maxScale = 2.5f;
        [SerializeField]
        private float scale = 1f;
        [SerializeField]
        private float speed = 1f;

        [Header("antistuck")]
        [SerializeField]
        private float stuckAngle = 6f;
        [SerializeField]
        private float antiStuckModeDelay = 10f;
        [SerializeField]
        private float antiStuckAngle = 7.5f;

        [SerializeField]
        private float lastBrickHitTime = float.MinValue;

        private IObjectResolver resolver = default;


        private void Awake()
        {
            Scale = scale;
        }

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
            Speed = resolver.Resolve<LevelController>().BallsSpeed;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent(out Brick brick))
            {
                brick.Heals -= Damage;
                lastBrickHitTime = Time.fixedTime;
            }
            else if(collision.collider.TryGetComponent(out Platform platform))
            {
                lastBrickHitTime = Time.fixedTime;
                resolver.Resolve<AudioSystem>().Play(platformHitSound);
            }
            Speed = speed;
        }

        public float Speed
        {
            get => speed;
            set
            {
                speed = value;
                Vector2 direction = body.velocity.normalized;
                body.velocity = direction * speed;
            }
        }

        public Vector2 Direction
        {
            get => body.velocity.normalized;
            set
            {
                body.velocity = value.normalized * Speed;
            }
        }

        public float Mass
        {
            get => body.mass;
            set => body.mass = value;
        }

        public float Scale
        {
            get => collider.radius;
            set
            {
                scale = Mathf.Clamp(value, minScale, maxScale);
                Vector3 transformScale = new Vector3(scale, scale, scale);
                transform.localScale = transformScale;
            }
        }

        public bool Simulated
        {
            get => body.simulated;
            set
            {
                if(value == body.simulated)
                {
                    return;
                }

                body.simulated = value;

                if(body.simulated)
                {
                    lastBrickHitTime = Time.fixedTime;
                }
            }
        }

        private void FixedUpdate()
        {
            if(body.simulated)
            {
                float currentAngle = CurrentAngle;
                if(lastBrickHitTime + antiStuckModeDelay < Time.fixedTime && Mathf.Abs(currentAngle) < stuckAngle)
                {
                    float newAngle = Mathf.Sign(currentAngle) * antiStuckAngle;
                    CurrentAngle = newAngle;
                }
            }
        }

        private float CurrentAngle
        {
            get
            {
                Vector2 v = body.velocity;
                float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
                return angle;
            }
            set
            {
                float rad = value * Mathf.Deg2Rad;
                float x = Mathf.Cos(rad) * speed;
                float y = Mathf.Sin(rad) * speed;
                body.velocity = new Vector2(x, y);
            }
        }


        public Rigidbody2D Body
        {
            get => body;
        }
        
        public Transform Pivot
        {
            get => pivot;
        }
    }
}