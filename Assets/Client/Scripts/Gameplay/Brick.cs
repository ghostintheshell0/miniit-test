using System;
using TriInspector;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class Brick : MonoBehaviour
    {
        public event Action<Brick> Dead = default;

        [SerializeField]
        private int heals = 1;

        [SerializeField]
        private int maxHeals = 1;

        [SerializeField][ReadOnly]
        private bool isDead = false;

        private void Die()
        {
            if(isDead)
            {
                return;
            }

            isDead = true;
            Dead?.Invoke(this);

            Destroy(gameObject);
        }

        public int Heals
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