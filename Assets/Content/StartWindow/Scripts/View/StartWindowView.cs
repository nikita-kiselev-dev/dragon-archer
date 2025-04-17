using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.StartWindow.Scripts.View
{
    public class StartWindowView : IView
    {
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_WebSiteButton;

        private UnityAction _onPlayButtonClicked;
        private UnityAction _onSettingsButtonClicked;
        private UnityAction _onWebSiteButtonClicked;
        
        public override void Init(IViewSignalManager viewSignalManager)
        {
            _onPlayButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.StartGameSignal);
            _onSettingsButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.OpenSettingsSignal);
            _onWebSiteButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.OpenWebSiteSignal);
            
            SetupButtons();
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