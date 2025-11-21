using UnityEngine;

namespace miniit.Arcanoid
{
    public abstract class BaseBonus : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if(collision.TryGetComponent(out Platform platform))
            {
                Apply(platform);
                Destroy(gameObject);
            }
        }

        protected virtual void Apply(Platform platform)
        {

        }
    }
}