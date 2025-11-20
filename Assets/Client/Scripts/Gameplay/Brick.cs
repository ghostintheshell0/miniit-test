using System;
using TriInspector;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class Brick : MonoBehaviour
    {
        public event Action<Brick> Dead = default;

        [SerializeField]
        protected int heals = 1;

        [SerializeField]
        protected int maxHeals = 1;

        [SerializeField][ReadOnly]
        protected bool isDead = false;

        protected virtual void Die()
        {
            if(isDead)
            {
                return;
            }

            isDead = true;
            Dead?.Invoke(this);

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
    }
}