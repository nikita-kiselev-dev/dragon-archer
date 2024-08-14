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

        private string _loadingLocalizedString;
        private Coroutine _textAnimationCoroutine;
        
        public MonoBehaviour MonoBehaviour => this;
        public CanvasGroup CanvasGroup => m_CanvasGroup;
        public GradientColor GradientColor => m_GradientColor;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
        
        private async void Awake()
        {
            _loadingLocalizedString = await "loading".LocalizeAsync();
        }

        private void OnEnable()
        {
            _textAnimationCoroutine = StartCoroutine(TextAnimation());
        }

        private void OnDisable()
        {
            if (_textAnimationCoroutine != null)
            {
                StopCoroutine(_textAnimationCoroutine);
            }
        }

        private IEnumerator TextAnimation()
        {
            while (true)
            {
                m_LoadingText.text = _loadingLocalizedString;
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