using UnityEngine;

namespace miniIT.Arcanoid
{
    public class VFXInstance : MonoBehaviour
    {
        public ParticleSystem[] particleSystems;
        [Tooltip("set value less than zero to make infinity lifetime")]
        public float lifetime = -1f;
        private float startTime = float.MinValue;

        public void Play()
        {
            startTime = Time.time;

            foreach (var ps in particleSystems)
            {
                ps.Play(true);
            }
        }

        public bool IsAlive()
        {
            if(lifetime < 0f)
            {
                return true;
            }

            return startTime + lifetime > Time.time;
        }
    }
}