using Content.LoadingCurtain.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Localization;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : ILoadingCurtainController
    {
        private readonly ISignalBus _signalBus;
        private readonly ILoadingCurtainView _view;
        private readonly IViewManager _viewManager;
        
        private IViewInteractor _viewInteractor;

        [Inject]
        public LoadingCurtainController(
            ISignalBus signalBus, 
            ILoadingCurtainView view, 
            IViewManager viewManager)
        {
            _signalBus = signalBus;
            _view = view;
            _viewManager = viewManager;
            
            Init();
        }
        
        private void Init()
        {
            RegisterAndInitView();
            ConfigureView().Forget();
            _signalBus.Subscribe<OnChangeSceneRequestSignal>(this, Show);
            _signalBus.Subscribe<OnPostInitPhaseCompletedSignal>(this, Hide);
        }
        
        private void Show()
        {
            _viewInteractor.Open();
        }

        private void Hide()
        {
            _viewInteractor.Close();
        }
        
        private void RegisterAndInitView()
        {
            var animator = new LoadingCurtainGradientColorAnimator(_view as LoadingCurtainView);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.LoadingCurtain)
                .SetViewType(ViewType.Service)
                .SetView(_view)
                .SetCustomAnimator(animator)
                .EnableFromStart()
                .RegisterAndInit();
        }

        private async UniTaskVoid ConfigureView()
        {
            var loadingLocalizedString = await "loading".LocalizeAsync();
            _view.SetLoadingText(loadingLocalizedString);
        }
    }
}