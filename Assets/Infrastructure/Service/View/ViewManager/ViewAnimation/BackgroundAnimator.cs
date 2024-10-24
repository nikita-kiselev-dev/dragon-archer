using DG.Tweening;
using Infrastructure.Service.View.ViewFactory;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewManager.ViewAnimation
{
    public class BackgroundAnimator : IViewAnimator
    {
        [Inject] private readonly ICanvasHandler _canvasHandler;
        
        private const float ShowViewBackgroundDuration = 0.2f;
        private const float HideViewBackgroundDuration = 0.2f;
        private const float ShowColorAlphaValue = 0.8f;
        
        public void Show()
        {
            var backgroundImage = _canvasHandler.PopupCanvasBackground;
            
            if (_canvasHandler.PopupCanvasBackground.gameObject.activeSelf)
            {
                return;
            }
            
            var sequence = DOTween.Sequence();
            sequence
                .PrependCallback(() =>
                {
                    var backgroundColor = backgroundImage.color;
                    backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
                    backgroundImage.gameObject.SetActive(true);
                })
                .Append(backgroundImage.DOFade(ShowColorAlphaValue, ShowViewBackgroundDuration));
        }

        public void Hide()
        {
            var backgroundImage = _canvasHandler.PopupCanvasBackground;
            
            if (!backgroundImage.gameObject.activeSelf)
            {
                return;
            }
            
            backgroundImage
                .DOFade(0f, HideViewBackgroundDuration)
                .OnComplete(() => backgroundImage.gameObject.SetActive(false));
        }
    }
}