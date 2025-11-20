using UnityEngine;
using UnityEngine.AddressableAssets;

namespace miniit.Arcanoid
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private AssetReference startScene;

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