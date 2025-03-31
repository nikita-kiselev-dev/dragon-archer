using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Content.DailyBonus.Scripts.View
{
    public class DailyBonusView : MonoBehaviour, IDailyBonusView
    {
        [SerializeField] private Button m_CloseButton;
        [SerializeField] private RewardRowsManager m_RewardRowsManager;
        
        private UnityAction _onCloseButtonClicked;

        public MonoBehaviour MonoBehaviour => this;
        public RewardRowsManager RewardRowsManager => m_RewardRowsManager;
        
        public void Init(IViewSignalManager viewSignalManager)
        {
            _onCloseButtonClicked = viewSignalManager.GetCloseSignal();
            
            m_CloseButton.onClick.RemoveAllListeners();
            m_CloseButton.onClick.AddListener(OnCloseButtonClicked);
        }
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }
        
        private void OnCloseButtonClicked()
        {
            _onCloseButtonClicked?.Invoke();
        }
    }
}