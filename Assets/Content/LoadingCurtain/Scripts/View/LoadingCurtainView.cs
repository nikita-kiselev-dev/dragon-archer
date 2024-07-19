using Infrastructure.Service.View;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts.View
{
    public class LoadingCurtainView : MonoBehaviour, IMonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;

        public CanvasGroup CanvasGroup => m_CanvasGroup;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}