using UnityEngine;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(fileName = "SoundSet", menuName = "Game/Sound Set")]
    public class SoundSet : ScriptableObject
    {
        public AudioClip[] clips;
    }

}