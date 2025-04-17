using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusView : IDailyBonusView
    {
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RewardRowsManager m_RewardRowsManager;
        
        private UnityAction _onCloseButtonClicked;

        public override RewardRowsManager RewardRowsManager => m_RewardRowsManager;
        
        public override void Init(IViewSignalManager viewSignalManager)
        {
            _onCloseButtonClicked = viewSignalManager.GetCloseSignal();
            
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        private void OnCloseButtonClicked()
        {
            _onCloseButtonClicked?.Invoke();
        }
    }
}