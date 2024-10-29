using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.SettingsPopup.Scripts.View
{
    public class SettingsPopupView : MonoBehaviour, ISettingsPopupView
    {
        [SerializeField] private Button m_CloseButton;
        
        [SerializeField] private Slider m_SoundsVolumeSlider;
        [SerializeField] private Slider m_MusicVolumeSlider;
 
        private UnityAction _onCloseButtonClicked;
        
        public MonoBehaviour MonoBehaviour => this;
        
        public void Init(IViewSignalManager viewSignalManager)
        {
            _onCloseButtonClicked = viewSignalManager.GetCloseSignal();
            
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(OnCloseButtonClicked);
            
            m_SoundsVolumeSlider.onValueChanged.RemoveAllListeners();
            var changeSoundsVolumeSignal = 
                viewSignalManager.GetSignal<float>(SettingsPopupSignals.SoundsVolumeChangedSignal, false);
            m_SoundsVolumeSlider.onValueChanged.AddListener(changeSoundsVolumeSignal);
            
            m_MusicVolumeSlider.onValueChanged.RemoveAllListeners();
            var changeMusicVolumeSignal = 
                viewSignalManager.GetSignal<float>(SettingsPopupSignals.MusicVolumeChangedSignal, false);
            m_MusicVolumeSlider.onValueChanged.AddListener(changeMusicVolumeSignal);
        }

        public void SetSoundsSliderValue(float value)
        {
            m_SoundsVolumeSlider.value = value;
        }

        public void SetMusicSliderValue(float value)
        {
            m_MusicVolumeSlider.value = value;
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnCloseButtonClicked()
        {
            _onCloseButtonClicked?.Invoke();
        }
    }
}