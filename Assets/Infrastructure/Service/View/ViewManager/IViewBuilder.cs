using System;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using Infrastructure.Service.View.ViewSignalManager;

namespace Infrastructure.Service.View.ViewManager
{
    public interface IViewBuilder
    {
        public IViewBuilder SetViewKey(string viewKey);
        public IViewBuilder SetViewType(string viewType);
        public IViewBuilder SetView(MonoView monoView);
        public IViewBuilder SetViewSignalManager(IViewSignalManager viewSignalManager);
        public IViewBuilder EnableFromStart();
        public IViewBuilder SetAfterOpenAction(Action action);
        public IViewBuilder SetAfterCloseAction(Action action);
        public IViewBuilder SetCustomAnimator(IViewAnimator animator);
        public IViewInteractor RegisterAndInit();
    }
}