using System;
using Content.LoadingCurtain.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Localization;
using Infrastructure.Service.Scopes;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    [ControlEntityOrder(nameof(BootstrapScope), (int)BootstrapSceneInitOrder.LoadingCurtain)]
    public class LoadingCurtainController : ControlEntity, ILoadingCurtainController, IDisposable
    {
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly ILoadingCurtainView _view;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        
        private IViewInteractor _viewInteractor;
        
        protected override UniTask Init()
        {
            RegisterAndInitView();
            ConfigureView().Forget();
            _signalBus.Subscribe<OnInitPhaseCompletedSignal>(this, Hide);
            
            return UniTask.CompletedTask;
        }
        
        public void Show()
        {
            _viewInteractor.Open();
        }

        public void Hide()
        {
            _viewInteractor.Close();
        }

        void IDisposable.Dispose()
        {
            _signalBus?.Unsubscribe<OnInitPhaseCompletedSignal>(this);
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