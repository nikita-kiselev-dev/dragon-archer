using System;
using Core.View.ViewManager.ViewAnimation;
using Core.View.ViewSignalManager;

namespace Core.View.ViewManager
{
    public interface IViewBuilder
    {
        public IViewBuilder SetViewKey(string viewKey);
        public IViewBuilder SetViewType(string viewType);
        public IViewBuilder SetView(MonoView monoView);
        public IViewBuilder SetViewSignalManager(IViewSignalManager viewSignalManager);
        public IViewBuilder EnableFromStart(bool status);
        public IViewBuilder SetAfterOpenAction(Action action);
        public IViewBuilder SetAfterCloseAction(Action action);
        public IViewBuilder SetCustomAnimator(IViewAnimator animator);
        public IViewInteractor RegisterAndInit();
    }
}