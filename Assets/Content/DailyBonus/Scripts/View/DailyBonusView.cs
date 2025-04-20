using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusView : IDailyBonusView
    {
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RewardRowsManager m_RewardRowsManager;

        public override RewardRowsManager RewardRowsManager => m_RewardRowsManager;
        
        public override void Init(IViewSignalManager viewSignalManager)
        {
            ConfigureButtons(viewSignalManager);
        }

        private void ConfigureButtons(IViewSignalManager viewSignalManager)
        {
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(viewSignalManager.GetCloseSignal().Invoke);
        }
    }
}