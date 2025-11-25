using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace miniIT.Arcanoid
{

    public class AudioSystem : MonoBehaviour
    {
        public AudioChannelSettings defaultChannelSettings = default;

        [SerializeField]
        private AudioMixerGroup sfxGroup = default;
        [SerializeField]
        private string sfxGroupName = default;
        [SerializeField]
        private AudioMixerGroup musicGroup = default;
        [SerializeField]
        private string musicGroupName = default;

        private int nextId = 1;
        private Dictionary<int, AudioSource> activeShots = new Dictionary<int, AudioSource>();
        private Dictionary<string, List<AudioSource>> channelSources = new Dictionary<string, List<AudioSource>>();
        private IObjectResolver resolver = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
            GameConfig config = resolver.Resolve<GameConfig>();
            SFXVolume = config.defaultSFXVolume;
            MusicVolume = config.defaultMusicvolume;
        }

        public AudioShot Play(SoundSet set)
        {
            return Play(set, defaultChannelSettings);
        }

        public AudioShot Play(AudioClip clip)
        {
            return Play(clip, defaultChannelSettings);
        }

        public AudioShot Play(SoundSet set, AudioChannelSettings settings)
        {
            if(set == null || set.clips.Length == 0)
            {
                return default;
            }

            AudioClip clip = set.clips[Random.Range(0, set.clips.Length)];
            return Play(clip, settings);
        }

        public AudioShot Play(AudioClip clip, AudioChannelSettings settings)
        {
            if(clip == null || settings == null)
            {
                return default;
            }

            if(!channelSources.ContainsKey(settings.channelName))
            {
                channelSources[settings.channelName] = new List<AudioSource>();
            }

            List<AudioSource> sources = channelSources[settings.channelName];

            if(sources.Count >= settings.maxSounds)
            {
                switch(settings.OverrideSoundsBehaviour)
                {
                    case OverrideSoundsBehaviour.IgnoreNew:
                        return default;
                    case OverrideSoundsBehaviour.StopOld:
                        AudioSource oldest = sources[0];
                        StopSource(oldest);
                        sources.RemoveAt(0);
                        break;
                    case OverrideSoundsBehaviour.None:
                        break;
                }
            }

            AudioSource src = resolver.Instantiate(settings.sourcePrefab, transform);
            src.clip = clip;
            src.outputAudioMixerGroup = settings.group;
            src.Play();

            int id = nextId++;
            activeShots[id] = src;
            sources.Add(src);

            return new AudioShot { id = id };
        }

        public void Stop(in AudioShot shot)
        {
            if(activeShots.TryGetValue(shot.id, out AudioSource src))
            {
                StopSource(src);
                activeShots.Remove(shot.id);

                foreach((string key, List<AudioSource> source) in channelSources)
                {
                    source.Remove(src);
                }
            }
        }

        public void SetVolume(in AudioShot shot, float volume = 1f)
        {
            if(activeShots.TryGetValue(shot.id, out AudioSource src))
            {
                src.volume = volume;
            }
        }

        public void StopAll()
        {
            foreach(AudioSource src in activeShots.Values)
            {
                StopSource(src);
            }
            activeShots.Clear();
            channelSources.Clear();
        }

        public void Stop(AudioChannelSettings channel)
        {
            if(channel == null)
            {
                return;
            }

            if(!channelSources.ContainsKey(channel.channelName))
            {
                return;
            }

            foreach(AudioSource src in channelSources[channel.channelName])
            {
                StopSource(src);
                
                foreach(KeyValuePair<int, AudioSource> kvp in activeShots)
                {
                    if(kvp.Value == src)
                    {
                        activeShots.Remove(kvp.Key);
                        break;
                    }
                }
            }
            channelSources[channel.channelName].Clear();
        }

        private void StopSource(AudioSource src)
        {
            if(src == null)
            {
                return;
            }

            src.Stop();
            Destroy(src.gameObject);
        }
        
        public AudioSource GetSource(in AudioShot shot)
        {
            if(activeShots.TryGetValue(shot.id, out AudioSource src))
            {
                return src;
            }
            
            return null;
        }

        public float SFXVolume
        {
            get => GetChannelVolume(sfxGroup, sfxGroupName);
            set
            {
                SetChannelVolume(sfxGroup, sfxGroupName, value);
            }
        }

        public float MusicVolume
        {
            get => GetChannelVolume(musicGroup, musicGroupName);
            set
            {
                SetChannelVolume(musicGroup, musicGroupName, value);
            }
        }

        private void SetChannelVolume(AudioMixerGroup group, string name, float volume)
        {
            float v = Mathf.Clamp(volume, 0.0001f, 1f);
            float dB = Mathf.Log10(v) * 20f;
            group.audioMixer.SetFloat(name, dB);
        }

        private float GetChannelVolume(AudioMixerGroup group, string name)
        {
            float dB;
            if(group.audioMixer.GetFloat(name, out dB))
            {
                return Mathf.Pow(10f, dB / 20f);
            }

            return 0f;
        }
    }

    public struct AudioShot
    {
        public int id;
    }

    public enum OverrideSoundsBehaviour
    {
        None,
        IgnoreNew,
        StopOld
    }
}