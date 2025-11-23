using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public abstract class BaseBonus : MonoBehaviour
    {
        protected IObjectResolver resolver = default;

        [Inject]
        public virtual void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

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