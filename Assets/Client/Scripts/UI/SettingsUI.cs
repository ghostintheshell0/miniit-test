using Doozy.Runtime.UIManager.Components;
using UnityEngine;
using VContainer;

namespace miniIT.Arcanoid
{
    public class SettingsUI : MonoBehaviour
    {
        [SerializeField]
        private UISlider musicSlider = default;

        [SerializeField]
        private UISlider sfxSlider = default;

        private AudioSystem audioSystem = default;

        [Inject]
        public void Inject(IObjectResolver resolver)
        {
            audioSystem = resolver.Resolve<AudioSystem>();

            musicSlider.SetValueWithoutNotify(audioSystem.MusicVolume);
            musicSlider.OnValueChangedCallback.AddListener(MusicValueChangeListener);

            sfxSlider.SetValueWithoutNotify(audioSystem.SFXVolume);
            sfxSlider.OnValueChangedCallback.AddListener(SFXValueChangeListener);
        }

        private void MusicValueChangeListener(float value)
        {
            audioSystem.MusicVolume = value;
        }

        private void SFXValueChangeListener(float value)
        {
            audioSystem.SFXVolume = value;
        }
    }
}