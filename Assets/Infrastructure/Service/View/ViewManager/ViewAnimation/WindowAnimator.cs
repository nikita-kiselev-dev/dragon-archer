using UnityEngine;

namespace Infrastructure.Service.View.ViewManager.ViewAnimation
{
    public class WindowAnimator : IViewAnimator
    {
        private readonly Transform _viewTransform;
        
        public WindowAnimator(Transform viewTransform)
        {
            _viewTransform = viewTransform;
        }
        
        public void Show()
        {
            _viewTransform.gameObject.SetActive(true);
        }

        public void Hide()
        {
            _viewTransform.gameObject.SetActive(false);
        }
    }
}