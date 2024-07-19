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
            var sequence = DOTween.Sequence();
            sequence
                .Append(_canvasGroup.DOFade(fadeValue, duration))
                .Pause();
            return sequence;
        }

        private Sequence ShowAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence
                .AppendCallback(() =>
                {
                    _canvasGroup.alpha = 0f;
                    _gameObject.SetActive(true);
                })
                .Append(FadeAnimation(1f, LoadingCurtainInfo.ShowAnimationDuration))
                .Pause();
            return sequence;
        }

        private Sequence HideAnimation()
        {
            var sequence = DOTween.Sequence();
            sequence
                .AppendCallback(() =>
                {
                    _canvasGroup.alpha = 1f;
                })
                .Append(FadeAnimation(0f, LoadingCurtainInfo.HideAnimationDuration))
                .AppendCallback(() =>
                {
                    _gameObject.SetActive(false);
                });
            return sequence;
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
            _currentSequence.Kill();
            _currentSequence = sequence;
            _currentSequence.Play();
        }
    }
}