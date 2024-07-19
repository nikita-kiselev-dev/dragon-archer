using Infrastructure.Service.View.ViewManager.ViewAnimation;
using Infrastructure.Service.View.ViewSignalManager;

namespace Infrastructure.Service.View.ViewManager
{
    public interface IViewBuilder
    {
        public IViewBuilder SetViewKey(string viewKey);
        public IViewBuilder SetViewType(string viewType);
        public IViewBuilder SetView(IMonoBehaviour view);
        public IViewBuilder SetViewSignalManager(IViewSignalManager viewSignalManager);
        public IViewBuilder EnableFromStart();
        public IViewBuilder SetCustomOpenAnimation(IViewAnimator animator);
        public IViewBuilder SetCustomCloseAnimation(IViewAnimator animator);
        public IViewInteractor RegisterAndInit();
    }
}