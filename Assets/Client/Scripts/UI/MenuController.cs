using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace miniIT.Arcanoid
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private SoundSet playlist = default;
        [SerializeField]
        private IObjectResolver resolver = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            this.resolver = resolver;
            resolver.Resolve<MusicPlayer>().Play(playlist);
        }

        public void Play()
        {
            PlayerService playerService = resolver.Resolve<PlayerService>();
            playerService.ResetPlayer();
            Addressables.LoadSceneAsync(playerService.GetLevel());
        }

        public void Exit()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }
    }
}