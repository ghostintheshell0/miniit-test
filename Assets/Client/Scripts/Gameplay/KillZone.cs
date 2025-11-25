using System;
using UnityEngine;

namespace miniIT.Arcanoid
{
    public class KillZone : MonoBehaviour
    {
        public event Action<Collider2D> ObjectEntered = default;

        private void OnTriggerEnter2D(Collider2D collider)
        {
            ObjectEntered?.Invoke(collider);
            Destroy(collider.gameObject);
        }
    }
}