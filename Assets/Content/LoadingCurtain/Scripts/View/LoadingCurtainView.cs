using System.Collections;
using Infrastructure.Service.Localization;
using Infrastructure.Service.View.UIEffects;
using TMPro;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts.View
{
    public class LoadingCurtainView : MonoBehaviour, ILoadingCurtainView
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_LoadingText;
        [SerializeField] private GradientColor m_GradientColor;

        private Coroutine _textAnimationCoroutine;
        
        public CanvasGroup CanvasGroup => m_CanvasGroup;
        public GradientColor GradientColor => m_GradientColor;

        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        private void OnEnable()
        {
            _textAnimationCoroutine = StartCoroutine(TextAnimation());
        }

        private void OnDisable()
        {
            StopCoroutine(_textAnimationCoroutine);
        }

        private IEnumerator TextAnimation()
        {
            while (true)
            {
                m_LoadingText.text = "loading".Localize();
                var dotsToAdd = LoadingCurtainInfo.DotsToAddInAnimation;

                while (dotsToAdd >= 0)
                {
                    yield return new WaitForSeconds(LoadingCurtainInfo.AddDotAnimationDelay);
                    m_LoadingText.text += ".";
                    dotsToAdd--;
                }
            }
        }
    }
}