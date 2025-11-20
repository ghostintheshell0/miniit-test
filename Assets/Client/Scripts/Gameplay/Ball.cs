using UnityEngine;

namespace miniit.Arcanoid
{
    public class Ball : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body = default;

        [SerializeField]
        private new CircleCollider2D collider = default;
        public float startSpeed = 5;
        public int Damage = 1;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.collider.TryGetComponent<Brick>(out var brick))
            {
                brick.Heals -= Damage;
            }
        }

        public float Speed
        {
            get => body.velocity.magnitude;
            set
            {
                Vector2 direction = body.velocity.normalized;
                body.velocity = direction * value;
            }
        }

        public float Mass
        {
            get => body.mass;
            set => body.mass = value;
        }

        public float Radius
        {
            get => collider.radius;
            set => collider.radius = value;
        }

        public Rigidbody2D Body
        {
            get => body;
        }
    }
}