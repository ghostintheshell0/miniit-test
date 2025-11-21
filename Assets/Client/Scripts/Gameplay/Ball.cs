using TMPro;
using UnityEngine;

namespace miniit.Arcanoid
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
        private TMP_Text DebugField = default;

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
        [SerializeField]
        private float minYSpeed = 0.1f;
        [SerializeField]
        private float antiStuckModeDelay = 10f;

        [SerializeField]
        private float lastBrickHitTime = float.MinValue;


        private void Awake()
        {
            Scale = scale;
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
            DebugField.text = $"{body.velocity.magnitude:F2}";
            if(body.simulated)
            {
                if(lastBrickHitTime + antiStuckModeDelay < Time.fixedTime)
                {
                    Vector2 velocity = body.velocity;
                    velocity.y += Mathf.Sign(velocity.y) * minYSpeed * Time.fixedDeltaTime;
                }
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