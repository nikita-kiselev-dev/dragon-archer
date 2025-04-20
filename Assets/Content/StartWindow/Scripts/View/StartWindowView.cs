using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.UI;

namespace Content.StartWindow.Scripts.View
{
    public class StartWindowMonoView : Infrastructure.Service.View.ViewManager.MonoView
    {
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Button m_SettingsButton;
        [SerializeField] private Button m_WebSiteButton;
        
        public override void Init(IViewSignalManager viewSignalManager)
        {
            ConfigureButtons(viewSignalManager);
        }

        private void ConfigureButtons(IViewSignalManager viewSignalManager)
        {
            m_PlayButton.onClick.RemoveAllListeners();
            m_PlayButton.onClick.AddListener(viewSignalManager.GetSignal(StartWindowInfo.StartGameSignal));
            
            m_SettingsButton.onClick.RemoveAllListeners();
            m_SettingsButton.onClick.AddListener(viewSignalManager.GetSignal(StartWindowInfo.OpenSettingsSignal));
            
            m_WebSiteButton.onClick.RemoveAllListeners();
            m_WebSiteButton.onClick.AddListener(viewSignalManager.GetSignal(StartWindowInfo.OpenWebSiteSignal));
        }
    }
}