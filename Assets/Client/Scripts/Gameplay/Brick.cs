using System;
using TriInspector;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{
    public class Brick : MonoBehaviour
    {
        public event Action<Brick> Dead = default;

        [SerializeField]
        private VFXInstance destroyVFXPrefab = default;
        [SerializeField]
        private SoundSet destroySounds = default;

        [SerializeField]
        protected int heals = 1;

        [SerializeField]
        protected int maxHeals = 1;

        [SerializeField][ReadOnly]
        protected bool isDead = false;

        public int pointPerKill = 1;
        public bool canBeIgnored = false;


        protected IObjectResolver resolver;

        [Inject]
        public virtual void Init(IObjectResolver resolver)
        {
            this.resolver = resolver;
        }

        protected virtual void Die()
        {
            if(isDead)
            {
                return;
            }

            isDead = true;
            Dead?.Invoke(this);

            resolver.Resolve<AudioSystem>().Play(destroySounds);
            if(DestroyVFXPrefab != default)
            {
                resolver.Resolve<VFXSpawner>().Spawn(DestroyVFXPrefab, transform.position, transform);
            }

            Destroy(gameObject);
        }

        public virtual int Heals
        {
            get => heals;
            set
            {
                heals = Mathf.Clamp(value, 0, maxHeals);
                if(heals == 0)
                {
                    Die();
                }
            }
        }

        public void Kill()
        {
            Die();
        }

        public VFXInstance DestroyVFXPrefab => destroyVFXPrefab;
    }

}