using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.SettingsPopup.Scripts.View
{
    public class SettingsPopupView : ISettingsPopupView
    {
        [SerializeField] private Button m_CloseButton;
        
        [SerializeField] private Slider m_SoundsVolumeSlider;
        [SerializeField] private Slider m_MusicVolumeSlider;
        
        public override void Init(
            UnityAction<float> onSoundsVolumeChanged, 
            UnityAction<float> onMusicVolumeChanged)
        {
            ConfigureButtons(onSoundsVolumeChanged, onMusicVolumeChanged);
        }

        public override void SetSoundsSliderValue(float value)
        {
            m_SoundsVolumeSlider.value = value;
        }

        public override void SetMusicSliderValue(float value)
        {
            m_MusicVolumeSlider.value = value;
        }

        private void ConfigureButtons(
            UnityAction<float> onSoundsVolumeChanged, 
            UnityAction<float> onMusicVolumeChanged)
        {
            m_CloseButton.onClick.RemoveListener(ViewInteractor.Close);
            m_CloseButton.onClick.AddListener(ViewInteractor.Close);
            
            m_SoundsVolumeSlider.onValueChanged.RemoveListener(onSoundsVolumeChanged);
            m_SoundsVolumeSlider.onValueChanged.AddListener(onSoundsVolumeChanged);
            
            m_MusicVolumeSlider.onValueChanged.RemoveListener(onMusicVolumeChanged);
            m_MusicVolumeSlider.onValueChanged.AddListener(onMusicVolumeChanged);
        }
    }
}