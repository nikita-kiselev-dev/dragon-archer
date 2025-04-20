using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.UI;

namespace Content.SettingsPopup.Scripts.View
{
    public class SettingsPopupView : ISettingsPopupView
    {
        [SerializeField] private Button m_CloseButton;
        
        [SerializeField] private Slider m_SoundsVolumeSlider;
        [SerializeField] private Slider m_MusicVolumeSlider;
        
        public override void Init(IViewSignalManager viewSignalManager)
        {
            ConfigureButtons(viewSignalManager);
        }

        public override void SetSoundsSliderValue(float value)
        {
            m_SoundsVolumeSlider.value = value;
        }

        public override void SetMusicSliderValue(float value)
        {
            m_MusicVolumeSlider.value = value;
        }

        private void ConfigureButtons(IViewSignalManager viewSignalManager)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(viewSignalManager.GetCloseSignal());
            
            m_SoundsVolumeSlider.onValueChanged.RemoveAllListeners();
            m_SoundsVolumeSlider.onValueChanged.AddListener(
                viewSignalManager.GetSignal<float>(
                    SettingsPopupSignals.SoundsVolumeChangedSignal, 
                    false));
            
            m_MusicVolumeSlider.onValueChanged.RemoveAllListeners();
            m_MusicVolumeSlider.onValueChanged.AddListener(
                viewSignalManager.GetSignal<float>(
                    SettingsPopupSignals.MusicVolumeChangedSignal, 
                    false));
        }
    }
}