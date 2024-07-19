using System.Collections.Generic;
using DG.Tweening;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewManager : IViewManager
    {
        [Inject] private readonly IMainCanvasController _mainCanvasController;
        
        private Dictionary<string, IViewWrapper> _viewWrappers = new Dictionary<string, IViewWrapper>();

        public void Open(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            var customAnimation = viewWrapper.CustomOpenAnimation;

            if (customAnimation != null)
            {
                customAnimation();
            }
            else
            {
                if (viewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return;
                }

                var viewType = viewWrapper.ViewType;
                var animator = GetAnimator(viewType, viewMonoBehaviour.transform);

                if (viewType == ViewType.Popup)
                {
                    ShowBackground();
                }
                
                animator.Show();
            }
        }

        public void Close(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            var customAnimation = viewWrapper.CustomCloseAnimation;

            if (customAnimation != null)
            {
                customAnimation();
            }
            else
            {
                if (viewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return;
                }

                var viewType = viewWrapper.ViewType;
                var animator = GetAnimator(viewType, viewMonoBehaviour.transform);
                
                if (viewType == ViewType.Popup)
                {
                    HideBackground();
                }
                
                animator.Hide();
            }
        }

        public void CloseAll()
        {
            foreach (var view in _viewWrappers)
            {
                Close(view.Value.ViewKey);
            }
        }

        public void RegisterView(IViewWrapper viewWrapper)
        {
            _viewWrappers.Add(viewWrapper.ViewKey, viewWrapper);
            ConfigureAfterRegistration(viewWrapper);
        }

        private IViewWrapper GetViewWrapper(string viewKey)
        {
            _viewWrappers.TryGetValue(viewKey, out var viewWrapper);
            return viewWrapper;
        }

        private void ConfigureAfterRegistration(IViewWrapper viewWrapper)
        {
            var view = viewWrapper.View;
            view.SetActive(viewWrapper.IsEnabledOnStart);
        }

        private IViewAnimator GetAnimator(string viewType, Transform transform)
        {
            if (viewType is ViewType.Window or ViewType.Service)
            {
                return new WindowAnimator(transform);
            }
            else
            {
                return new PopupAnimator(transform);
            }
        }

        private void ShowBackground()
        {
            var popupBackground = _mainCanvasController.GetPopupBackground();

            if (popupBackground.gameObject.activeSelf)
            {
                return;
            }
            
            var sequence = DOTween.Sequence();
            sequence
                .PrependCallback(() =>
                {
                    var backgroundColor = popupBackground.color;
                    popupBackground.color = new Color(backgroundColor.r, backgroundColor.g, backgroundColor.b, 0);
                    popupBackground.gameObject.SetActive(true);
                })
                .Append(popupBackground.DOFade(0.5f, ViewInfo.ShowViewBackgroundDuration));
        }

        private void HideBackground()
        {
            var popupBackground = _mainCanvasController.GetPopupBackground();
            
            if (!popupBackground.gameObject.activeSelf)
            {
                return;
            }
            
            popupBackground
                .DOFade(0f, ViewInfo.HideViewBackgroundDuration)
                .OnComplete(() => popupBackground.gameObject.SetActive(false));
        }
    }
}