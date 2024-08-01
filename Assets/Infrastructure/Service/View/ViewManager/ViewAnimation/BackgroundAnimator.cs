using DG.Tweening;
using Infrastructure.Service.View.Canvas;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Infrastructure.Service.View.ViewManager.ViewAnimation
{
    public class BackgroundAnimator : IViewAnimator
    {
        [Inject] private readonly IMainCanvasController _mainCanvasController;
        
        private const float ShowViewBackgroundDuration = 0.2f;
        private const float HideViewBackgroundDuration = 0.2f;
        private const float ShowColorAlphaValue = 0.8f;

        private Image _backgroundImage;
        
        public void Show()
        {
            _backgroundImage = _mainCanvasController.GetPopupBackground();
            
            if (_backgroundImage.gameObject.activeSelf)
            {
                return;
            }
            
            var sequence = DOTween.Sequence();
            sequence
                .PrependCallback(() =>
                {
                    var backgroundColor = _backgroundImage.color;
                    _backgroundImage.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
                    _backgroundImage.gameObject.SetActive(true);
                })
                .Append(_backgroundImage.DOFade(ShowColorAlphaValue, ShowViewBackgroundDuration));
        }

        public void Hide()
        {
            _backgroundImage = _mainCanvasController.GetPopupBackground();
            
            if (!_backgroundImage.gameObject.activeSelf)
            {
                return;
            }
            
            _backgroundImage
                .DOFade(0f, HideViewBackgroundDuration)
                .OnComplete(() => _backgroundImage.gameObject.SetActive(false));
        }
    }
}