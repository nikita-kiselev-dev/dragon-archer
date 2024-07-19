using Content.LoadingCurtain.Scripts.View;
using DG.Tweening;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts
{
    public class LoadingCurtainViewAnimator : IViewAnimator
    {
        private readonly GameObject _gameObject;
        private readonly CanvasGroup _canvasGroup;
        
        public LoadingCurtainViewAnimator(LoadingCurtainView view)
        {
            _gameObject = view.gameObject;
            _canvasGroup = view.CanvasGroup;
        }
        
        private Sequence _currentSequence;
        private Sequence _showSequence;
        private Sequence _hideSequence;
        
        public void Show()
        {
            _showSequence = ShowAnimation();
            PlayNowOrNext(_showSequence);
        }

        public void Hide()
        {
            _hideSequence = HideAnimation();
            PlayNowOrNext(_hideSequence);
        }

        private Sequence FadeAnimation(float fadeValue, float duration)
        {
            return DOTween
                .Sequence()
                .Append(_canvasGroup.DOFade(fadeValue, duration))
                .Pause();
        }

        private Sequence ShowAnimation()
        {
            return DOTween
                .Sequence()
                .AppendCallback(() =>
                {
                    _canvasGroup.alpha = 0f;
                    _gameObject.SetActive(true);
                })
                .Append(FadeAnimation(1f, LoadingCurtainInfo.ShowAnimationDuration))
                .Pause();
        }

        private Sequence HideAnimation()
        {
            return DOTween
                .Sequence()
                .AppendCallback(() =>
                {
                    _canvasGroup.alpha = 1f;
                })
                .Append(FadeAnimation(0f, LoadingCurtainInfo.HideAnimationDuration))
                .AppendCallback(() =>
                {
                    _gameObject.SetActive(false);
                });
        }
        
        private void PlayNowOrNext(Sequence sequence)
        {
            if (_currentSequence != null && _currentSequence.IsActive())
            {
                _currentSequence.OnComplete(() =>
                {
                    SetCurrentAndPlay(sequence);
                });
            }
            else
            {
                SetCurrentAndPlay(sequence);
            }
        }

        private void SetCurrentAndPlay(Sequence sequence)
        {
            _currentSequence?.Kill();
            _currentSequence = sequence;
            _currentSequence?.Play();
        }
    }
}