using Content.LoadingCurtain.Scripts.View;
using Infrastructure.Service.Localization;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : ILoadingCurtainController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;

        private ILoadingCurtainView _view;
        private IViewInteractor _viewInteractor;
        
        public void Init()
        {
            RegisterAndInitView();
            ConfigureView();
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
            _view = _viewFactory
                .CreateView<ILoadingCurtainView>(ViewInfo.LoadingCurtain, ViewType.Service);
            
            var animator = new LoadingCurtainViewAnimator(_view);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.LoadingCurtain)
                .SetViewType(ViewType.Service)
                .SetView(_view)
                .SetCustomOpenAnimation(animator)
                .SetCustomCloseAnimation(animator)
                .EnableFromStart()
                .RegisterAndInit();
        }

        private void ConfigureView()
        {
            var loadingLocalizedString = "loading".LocalizeAsync();
            _view.SetLoadingText(loadingLocalizedString);
        }
    }
}