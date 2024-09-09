using System;
using Infrastructure.Service.View.ViewManager.ViewAnimation;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewWrapper : IViewWrapper
    {
        public string ViewKey { get; set; }
        public string ViewType { get; set; }
        public IMonoBehaviour View { get; set; }
        public bool IsEnabledOnStart { get; set; }
        public Action AfterOpenAction { get; set; }
        public Action AfterCloseAction { get; set; }
        public IViewAnimator ViewAnimator { get; set; }
    }
}