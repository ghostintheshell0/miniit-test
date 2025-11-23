using UnityEngine;

namespace miniIT.Arcanoid
{
    public class Fall : MonoBehaviour
    {
        [SerializeField]
        private Rigidbody2D body;

        [SerializeField]
        private float speed;

        private void Start()
        {
            body.velocity = Vector2.down * speed;
        }
    }
}