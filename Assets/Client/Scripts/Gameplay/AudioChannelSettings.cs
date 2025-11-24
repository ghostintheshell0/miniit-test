using UnityEngine;
using UnityEngine.Audio;

namespace miniIT.Arcanoid
{
    [CreateAssetMenu(menuName = "Game/AudioChannelSettings")]
    public class AudioChannelSettings : ScriptableObject
    {
        public string channelName = "sfx";
        public AudioSource sourcePrefab = default;
        public AudioMixerGroup group = default;
        public int maxSounds = 6;
        public OverrideSoundsBehaviour OverrideSoundsBehaviour = OverrideSoundsBehaviour.None;
    }
}