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
        private TMP_Text DebugField = default;

        public float startSpeed = 5;
        public int Damage = 1;

        [SerializeField]
        private float defaultRadius = 1f;
        
        [SerializeField]
        private float minScale = 0.5f;

        [SerializeField]
        private float maxScale = 2.5f;
        [SerializeField]
        private float scale = 1f;
        [SerializeField]
        private float speed;

        private void Awake()
        {
            Scale = scale;
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent<Brick>(out var brick))
            {
                brick.Heals -= Damage;
            }
            Speed = speed;

        }

        public float Speed
        {
            get => body.velocity.magnitude;
            set
            {
                speed = value;
                Vector2 direction = body.velocity.normalized;
                body.velocity = direction * value;
            }
        }
        public Vector2 Direction
        {
            get => body.velocity.normalized;
            set
            {
                body.velocity = value * Speed;
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

        private void Update()
        {
            DebugField.text = $"{Speed:F2}";
        }

        public Rigidbody2D Body
        {
            get => body;
        }
    }
}