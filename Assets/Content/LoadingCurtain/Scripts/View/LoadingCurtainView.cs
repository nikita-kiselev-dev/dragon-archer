using System.Collections;
using Infrastructure.Service.Localization;
using Infrastructure.Service.View;
using TMPro;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts.View
{
    public class LoadingCurtainView : MonoBehaviour, IMonoBehaviour
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_LoadingText;

        private Coroutine _textAnimationCoroutine;
        
        public CanvasGroup CanvasGroup => m_CanvasGroup;

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