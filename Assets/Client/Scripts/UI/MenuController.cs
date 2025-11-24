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

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            resolver.Resolve<MusicPlayer>().Play(playlist);
        }

        public void Play()
        {
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