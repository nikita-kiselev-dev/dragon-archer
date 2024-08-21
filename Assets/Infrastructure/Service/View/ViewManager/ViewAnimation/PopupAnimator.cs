using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Service.View.ViewManager.ViewAnimation
{
    public class PopupAnimator : IViewAnimator
    {
        private const float PopInDuration = 0.8f;
        private const float PopOutDuration = 0.2f;
            
        private readonly Transform _viewTransform;
        
        public PopupAnimator(Transform viewTransform)
        {
            _viewTransform = viewTransform;
        }
        
        public void Show()
        {
            var sequence = DOTween.Sequence();

            sequence
                .PrependCallback(() =>
                {
                    _viewTransform.localScale = Vector3.zero;
                    _viewTransform.gameObject.SetActive(true);
                })
                .Append(_viewTransform.DOScale(Vector3.one, PopInDuration)).SetEase(Ease.OutBack);
        }

        public void Hide()
        {
            var sequence = DOTween.Sequence();
            
            sequence
                .Append(_viewTransform.DOScale(Vector3.zero, PopOutDuration / 1.2f))
                .AppendCallback(() =>
                {
                    _viewTransform.gameObject.SetActive(false);
                });
        }
    }
}