using Content.LoadingCurtain.Scripts.View;
using Infrastructure.Service.View;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : ILoadingCurtainController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        
        private IViewInteractor _viewInteractor;
        
        public void Init()
        {
            RegisterAndInitView();
        }
        
        public void Show()
        {
            _viewInteractor.Open();
        }

        public void Hide()
        {
            _viewInteractor.Close();
        }
        
        private void RegisterAndInitView()
        {
            var view = _viewFactory
                .CreateView<IMonoBehaviour>(ViewInfo.LoadingCurtain, ViewType.Service);
            
            var animator = new LoadingCurtainViewAnimator(view as ILoadingCurtainView);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.LoadingCurtain)
                .SetViewType(ViewType.Service)
                .SetView(view)
                .SetCustomOpenAnimation(animator)
                .SetCustomCloseAnimation(animator)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}