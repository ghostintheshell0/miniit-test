using System;
using UnityEngine;

namespace miniit.Arcanoid
{
    public class KillZone : MonoBehaviour
    {
        public event Action<Collider2D> ObjectEntered;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ObjectEntered?.Invoke(collider);
            Destroy(collider.gameObject);
        }
    }
}