using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace miniIT.Arcanoid
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private AssetReference startScene = default;

        [SerializeField]
        private SoundSet playlist = default;
        [SerializeField]
        private IObjectResolver resolver = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            Debug.Log("Hello?");
            this.resolver = resolver;
            resolver.Resolve<MusicPlayer>().Play(playlist);
        }

        public void Play()
        {
            resolver.Resolve<PlayerService>().ResetPlayer();
            Addressables.LoadSceneAsync(startScene);
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