using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.StartWindow.Scripts.View
{
    public class StartWindowView : MonoBehaviour, IView
    {
        [SerializeField] private Button m_StartButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_WebSiteButton;

        private UnityAction _onStartButtonClicked;
        private UnityAction _onSettingsButtonClicked;
        private UnityAction _onWebSiteButtonClicked;

        public void Init(IViewSignalManager viewSignalManager)
        {
            _onStartButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.StartGameSignal);
            _onSettingsButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.OpenSettingsSignal);
            _onWebSiteButtonClicked = viewSignalManager.GetSignal(StartWindowInfo.OpenWebSiteSignal);
            
            SetupButtons();
            
        }

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void SetupButtons()
        {
            m_StartButton.onClick.RemoveAllListeners();
            m_StartButton.onClick.AddListener(OnStartButtonClicked);
            
            m_SettingsButton.onClick.RemoveAllListeners();
            m_SettingsButton.onClick.AddListener(OnSettingsButtonClicked);
            
            m_WebSiteButton.onClick.RemoveAllListeners();
            m_WebSiteButton.onClick.AddListener(OnWebSiteButtonClicked);
        }

        private void OnStartButtonClicked()
        {
            _onStartButtonClicked?.Invoke();
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