using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.StartScreen.Scripts.View
{
    public class StartScreenView : MonoBehaviour, IView
    {
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_WebSiteButton;

        private UnityAction _onPlayButtonClicked;
        private UnityAction _onSettingsButtonClicked;
        private UnityAction _onWebSiteButtonClicked;

        public MonoBehaviour MonoBehaviour => this;
        
        public void Init(IViewSignalManager viewSignalManager)
        {
            _onPlayButtonClicked = viewSignalManager.GetSignal(StartScreenInfo.StartGameSignal);
            _onSettingsButtonClicked = viewSignalManager.GetSignal(StartScreenInfo.OpenSettingsSignal);
            _onWebSiteButtonClicked = viewSignalManager.GetSignal(StartScreenInfo.OpenWebSiteSignal);
            
            SetupButtons();
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void SetupButtons()
        {
            m_PlayButton.onClick.RemoveAllListeners();
            m_PlayButton.onClick.AddListener(OnPlayButtonClicked);
            
            m_SettingsButton.onClick.RemoveAllListeners();
            m_SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
            
            m_WebSiteButton.onClick.RemoveAllListeners();
            m_WebSiteButton.onClick.AddListener(OnWebSiteButtonClicked);
        }

        private void OnPlayButtonClicked()
        {
            _onPlayButtonClicked?.Invoke();
        }

        private void OnSettingsButtonClicked()
        {
            _onSettingsButtonClicked?.Invoke();
        }

        private void OnWebSiteButtonClicked()
        {
            _onWebSiteButtonClicked?.Invoke();
        }
    }
}