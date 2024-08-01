using System.Collections.Generic;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewManager : IViewManager
    {
        [Inject] private readonly IViewAnimator _backgroundAnimator;
         
        private readonly Dictionary<string, IViewWrapper> _viewWrappers = new();

        private IViewWrapper _lastViewWrapper;

        public void Open(string viewKey)
        {
            _lastViewWrapper = GetViewWrapper(viewKey);
            var customAnimation = _lastViewWrapper.CustomOpenAnimation;

            if (customAnimation != null)
            {
                customAnimation();
            }
            else
            {
                if (_lastViewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return;
                }

                var viewType = _lastViewWrapper.ViewType;
                var animator = GetAnimator(viewType, viewMonoBehaviour.transform);

                if (viewType == ViewType.Popup)
                {
                    _backgroundAnimator.Show();
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
                    _backgroundAnimator.Hide();
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

        public void CloseLast()
        {
            if (_lastViewWrapper != null)
            {
                Close(_lastViewWrapper.ViewKey);
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
    }
}