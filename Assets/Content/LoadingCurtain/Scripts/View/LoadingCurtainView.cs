using System.Collections;
using Infrastructure.Service.View.UIEffects;
using TMPro;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts.View
{
    public class LoadingCurtainView : ILoadingCurtainView
    {
        [SerializeField] private CanvasGroup m_CanvasGroup;
        [SerializeField] private TextMeshProUGUI m_LoadingText;
        [SerializeField] private GradientColor m_GradientColor;

        private readonly string[] _states = { "", ".", "..", "..." };
        private string _loadingLocalizedString;
        private int _dotCount;
        private Coroutine _textAnimationCoroutine;
        
        public CanvasGroup CanvasGroup => m_CanvasGroup;
        public GradientColor GradientColor => m_GradientColor;
        
        public override void SetLoadingText(string text)
        {
            _loadingLocalizedString = text;
        }

        private void OnEnable()
        {
            if (!string.IsNullOrEmpty(_loadingLocalizedString))
            {
                _textAnimationCoroutine = StartCoroutine(TextAnimation());
            }
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
                m_LoadingText.text = _loadingLocalizedString + _states[_dotCount];
                _dotCount = (_dotCount + 1) % 4;
                yield return new WaitForSeconds(LoadingCurtainConstants.AddDotAnimationDelay);
            }
        }
    }
}