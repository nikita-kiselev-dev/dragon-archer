using System;
using Content.LoadingCurtain.Scripts.View;
using DG.Tweening;
using Infrastructure.Service.View.UIEffects;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;

namespace Content.LoadingCurtain.Scripts
{
    public class LoadingCurtainGradientColorAnimator : IViewAnimator
    {
        private readonly GameObject _gameObject;
        private readonly CanvasGroup _canvasGroup;
        private readonly GradientColor _gradientColor;

        private readonly Color _topColor;
        private readonly Color _bottomColor;

        private readonly Action _afterShowCallback;
        private readonly Action _afterHideCallback;
        
        public LoadingCurtainGradientColorAnimator(
            LoadingCurtainView view, 
            Action afterShowCallback,
            Action afterHideCallback)
        {
            _gameObject = view.gameObject;
            _canvasGroup = view.CanvasGroup;
            _gradientColor = view.GradientColor;
            _topColor = _gradientColor.colorTop;
            _bottomColor = _gradientColor.colorBottom;
            _afterShowCallback = afterShowCallback;
            _afterHideCallback = afterHideCallback;
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

        private Sequence GradientColorChange(float duration, bool isReversed = false)
        {
            var topColor = isReversed ? _topColor : _bottomColor;
            var bottomColor = isReversed ? _bottomColor : _topColor;

            return DOTween
                .Sequence()
                .Join(DOTween.To(() => _gradientColor.colorTop, x => 
                    _gradientColor.colorTop = x, topColor, duration))
                .Join(DOTween.To(() => _gradientColor.colorBottom, x => 
                    _gradientColor.colorBottom = x, bottomColor, duration));
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
                .Append(FadeAnimation(1f, LoadingCurtainInfo.FadeInAnimationDuration))
                .Append(GradientColorChange(LoadingCurtainInfo.ColorChangeShowAnimationDuration))
                .AppendCallback(() => _afterShowCallback?.Invoke())
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
                .Append(GradientColorChange(LoadingCurtainInfo.ColorChangeHideAnimationDuration, true))
                .Append(FadeAnimation(0f, LoadingCurtainInfo.FadeOutAnimationDuration))
                .AppendCallback(() => _gameObject.SetActive(false))
                .AppendCallback(() => _afterHideCallback?.Invoke())
                .Pause();
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