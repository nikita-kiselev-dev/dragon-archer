using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.StartWindow.Scripts.View
{
    public class StartWindowView : Infrastructure.Service.View.ViewManager.MonoView
    {
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_WebSiteButton;
        
        public void Init(
            UnityAction onPlayButtonClicked, 
            UnityAction onSettingsButtonClicked, 
            UnityAction onWebSiteButtonClicked)
        {
            ConfigureButtons(
                onPlayButtonClicked, 
                onSettingsButtonClicked, 
                onWebSiteButtonClicked);
        }

        private void ConfigureButtons(
            UnityAction onPlayButtonClicked, 
            UnityAction onSettingsButtonClicked, 
            UnityAction onWebSiteButtonClicked)
        {
            m_PlayButton.onClick.RemoveAllListeners();
            m_PlayButton.onClick.AddListener(onPlayButtonClicked);
            
            m_SettingsButton.onClick.RemoveAllListeners();
            m_SettingsButton.onClick.AddListener(onSettingsButtonClicked);
            
            m_WebSiteButton.onClick.RemoveAllListeners();
            m_WebSiteButton.onClick.AddListener(onWebSiteButtonClicked);
        }
    }
}