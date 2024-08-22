using System.Threading;
using Cysharp.Threading.Tasks;
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
        private CancellationTokenSource _cancellationTokenSource;
        
        public MonoBehaviour MonoBehaviour => this;
        public CanvasGroup CanvasGroup => m_CanvasGroup;
        public GradientColor GradientColor => m_GradientColor;
        
        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }

        public void SetLoadingText(string text)
        {
            _loadingLocalizedString = text;
        }

        private void OnEnable()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            TextAnimation(_cancellationTokenSource.Token).Forget();
        }

        private void OnDisable()
        {
            _cancellationTokenSource?.Cancel(); 
        }
        
        private async UniTask TextAnimation(CancellationToken cancellationToken)
        {
            var loadingLocalizedTextReady = !string.IsNullOrEmpty(_loadingLocalizedString);
            
            await UniTask.WaitUntil(
                () => loadingLocalizedTextReady, 
                cancellationToken: cancellationToken);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                m_LoadingText.text = _loadingLocalizedString;
                var dotsToAdd = LoadingCurtainInfo.DotsToAddInAnimation;

                while (dotsToAdd >= 0)
                {
                    await UniTask.WaitForSeconds(
                        LoadingCurtainInfo.AddDotAnimationDelay, 
                        cancellationToken: cancellationToken);
                    
                    m_LoadingText.text += ".";
                    dotsToAdd--;
                }
            }
        }
    }
}