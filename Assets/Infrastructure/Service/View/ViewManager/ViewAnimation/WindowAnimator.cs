using System;
using UnityEngine;

namespace Infrastructure.Service.View.ViewManager.ViewAnimation
{
    public class WindowAnimator : IViewAnimator
    {
        private readonly Transform _viewTransform;
        private readonly Action _afterShowAction;
        private readonly Action _afterHideAction;
        
        public WindowAnimator(Transform viewTransform, Action afterShowAction = null, Action afterHideAction = null)
        {
            _viewTransform = viewTransform;
            _afterShowAction = afterShowAction;
            _afterHideAction = afterHideAction;
        }
        
        public void Show()
        {
            _viewTransform.gameObject.SetActive(true);
            _afterShowAction?.Invoke();
        }

        public void Hide()
        {
            _viewTransform.gameObject.SetActive(false);
            _afterHideAction?.Invoke();
        }
    }
}