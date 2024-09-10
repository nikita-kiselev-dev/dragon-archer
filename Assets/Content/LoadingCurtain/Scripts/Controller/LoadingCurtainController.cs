using System;
using System.Collections.Generic;
using System.Threading;
using Content.LoadingCurtain.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.GameManager;
using Infrastructure.Service.Localization;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Content.LoadingCurtain.Scripts.Controller
{
    public class LoadingCurtainController : ILoadingCurtainController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly ISignalBus _signalBus;

        private readonly List<UniTask> _loadingOperations = new();
        
        private ILoadingCurtainView _view;
        private IViewInteractor _viewInteractor;
        
        public async UniTask Init()
        {
            await RegisterAndInitView();
            ConfigureView().Forget();
            _signalBus.Subscribe<AddLoadingOperationSignal, UniTask>(this, AddLoadingOperation);
            _signalBus.Subscribe<OnGameManagerStartedSignal>(this, () => _ = Hide());
        }
        
        public void Show()
        {
            _loadingOperations.Clear();
            _viewInteractor.Open();
        }

        public async UniTaskVoid Hide()
        {
            await UniTask
                .WhenAll(_loadingOperations)
                .TimeoutWithoutException(TimeSpan.FromSeconds(LoadingCurtainInfo.LoadingTimeoutInSeconds));
            
            _loadingOperations.Clear();
            _viewInteractor.Close();
        }

        private void AddLoadingOperation(UniTask loadingOperation)
        {
            _loadingOperations.Add(loadingOperation);
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