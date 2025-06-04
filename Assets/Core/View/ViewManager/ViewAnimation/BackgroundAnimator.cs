using Core.View.Canvas.Scripts;
using DG.Tweening;
using UnityEngine;

namespace Core.View.ViewManager.ViewAnimation
{
    public class BackgroundAnimator : IViewAnimator
    {
        private ICanvasHandler _canvasHandler;
        
        private const float ShowViewBackgroundDuration = 0.2f;
        private const float HideViewBackgroundDuration = 0.2f;
        private const float ShowColorAlphaValue = 0.8f;
        
        public void SetCanvasManager(ICanvasHandler canvasHandler)
        {
            _canvasHandler = canvasHandler;
        }
        
        public void Show()
        {
            if (_canvasHandler.PopupCanvas is null || _canvasHandler.PopupCanvas.BackgroundImage.gameObject.activeSelf) return;
            var popupBackground = _canvasHandler.PopupCanvas.BackgroundImage;
            
            DOTween
                .Sequence()
                .PrependCallback(() =>
                {
                    var backgroundColor = popupBackground.color;
                    popupBackground.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
                    popupBackground.gameObject.SetActive(true);
                })
                .Append(popupBackground.DOFade(ShowColorAlphaValue, ShowViewBackgroundDuration));
        }

        public void Hide()
        {
            if (_canvasHandler.PopupCanvas is null || !_canvasHandler.PopupCanvas.BackgroundImage.gameObject.activeSelf) return;
            var popupBackground = _canvasHandler.PopupCanvas.BackgroundImage;
            
            popupBackground
                .DOFade(0f, HideViewBackgroundDuration)
                .OnComplete(() => popupBackground.gameObject.SetActive(false));
        }
    }
}