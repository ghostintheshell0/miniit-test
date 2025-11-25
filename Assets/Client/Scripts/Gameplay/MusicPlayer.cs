using System.Collections;
using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class MusicPlayer : MonoBehaviour
    {
        [Inject][HideInInspector]
        private AudioSystem audioSystem = default;
        [SerializeField]
        private AudioChannelSettings musicChannel = default;
        private SoundSet playlist = default;
        public float fadeDuration = 2f;

        private int currentIndex = 0;
        private AudioShot currentShot;
        private AudioSource currentSource;
        private Coroutine playlistRoutine;

        public void Play(SoundSet newPlaylist)
        {
            if(newPlaylist == null || newPlaylist.clips.Length == 0)
            {
                return;
            }

            if(playlist == newPlaylist)
            {
                return;
            }

            if(playlistRoutine != null)
            {
                StopCoroutine(playlistRoutine);
                if(currentSource != null)
                {
                    StartCoroutine(FadeOutAndStop(currentShot, currentSource, fadeDuration));
                }
            }

            // переключаемся на новый плейлист
            playlist = newPlaylist;
            currentIndex = 0;
            playlistRoutine = StartCoroutine(PlayPlaylist());
        }

        private IEnumerator PlayPlaylist()
        {
            while (true)
            {
                AudioClip clip = playlist.clips[currentIndex];

                currentShot = audioSystem.Play(clip, musicChannel);
                currentSource = audioSystem.GetSource(currentShot);
                if(currentSource == null)
                {
                    yield break;
                }

                yield return StartCoroutine(FadeVolume(currentSource, 0f, 1f, fadeDuration));

                yield return new WaitForSeconds(clip.length - fadeDuration);

                AudioClip nextClip = GetNextClip();

                AudioShot nextShot = audioSystem.Play(nextClip, musicChannel);
                AudioSource nextSource = audioSystem.GetSource(nextShot);

                yield return StartCoroutine(CrossFade(currentSource, nextSource, fadeDuration));

                audioSystem.Stop(currentShot);

                currentShot = nextShot;
                currentSource = nextSource;
            }
        }

        private AudioClip GetNextClip()
        {
            if(currentIndex >= playlist.clips.Length)
            {
                currentIndex = 0;
            }

            AudioClip result = playlist.clips[currentIndex];
            currentIndex++;
            return result;
        }

        private IEnumerator FadeVolume(AudioSource src, float from, float to, float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.fixedDeltaTime;
                src.volume = Mathf.Lerp(from, to, t / duration);
                yield return default;
            }
            src.volume = to;
        }

        private IEnumerator CrossFade(AudioSource from, AudioSource to, float duration)
        {
            float t = 0f;
            while (t < duration)
            {
                t += Time.fixedDeltaTime;
                float progress = t / duration;
                from.volume = Mathf.Lerp(1f, 0f, progress);
                to.volume = Mathf.Lerp(0f, 1f, progress);
                yield return null;
            }
            from.volume = 0f;
            to.volume = 1f;
        }

        private IEnumerator FadeOutAndStop(AudioShot shot, AudioSource src, float duration)
        {
            float startVolume = src.volume;
            float t = 0f;
            while (t < duration)
            {
                t += Time.fixedDeltaTime;
                if(src == default)
                {
                    audioSystem.Stop(shot);
                    yield break;
                }
                src.volume = Mathf.Lerp(startVolume, 0f, t / duration);
                yield return null;
            }
            audioSystem.Stop(shot);
        }
    }
}