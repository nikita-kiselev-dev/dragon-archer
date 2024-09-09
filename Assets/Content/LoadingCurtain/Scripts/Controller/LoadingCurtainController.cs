using System.Collections.Generic;
using Content.LoadingCurtain.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.GameManager;
using Infrastructure.Service.Localization;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : ILoadingCurtainController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly ISignalBus _signalBus;

        private readonly List<UniTask> _loadingOperationQueue = new();
        
        private ILoadingCurtainView _view;
        private IViewInteractor _viewInteractor;

        private bool _onBootstrap = true;
        
        public async UniTaskVoid Init()
        {
            await RegisterAndInitView();
            ConfigureView().Forget();
            _signalBus.Subscribe<AddLoadingOperationSignal, UniTask>(this, AddLoadingOperation);
            _signalBus.Subscribe<OnGameManagerStartedSignal>(this, () => _ = Hide());
        }
        
        public void Show()
        {
            _loadingOperationQueue.Clear();
            _viewInteractor.Open();
        }

        public async UniTaskVoid Hide()
        {
            await WaitForOperationsCompletion();
            _loadingOperationQueue.Clear();
            _viewInteractor.Close();
        }
        
        public void AddLoadingOperation(UniTask loadingOperation)
        {
            _loadingOperationQueue.Add(loadingOperation);
        }
        
        public async UniTask WaitForOperationsCompletion()
        {
            if (_onBootstrap)
            {
                _onBootstrap = false;
            }
            else
            {
                await UniTask.WhenAll(_loadingOperationQueue);
            }
        }
        
        private async UniTask RegisterAndInitView()
        {
            _view = await _viewFactory
                .CreateView<ILoadingCurtainView>(ViewInfo.LoadingCurtain, ViewType.Service);
            
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