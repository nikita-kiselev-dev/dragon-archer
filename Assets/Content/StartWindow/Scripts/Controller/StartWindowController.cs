using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Game;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Audio;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SceneStateMachine;
using Infrastructure.Service.SceneStateMachine.SceneStates;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using UnityEngine;
using VContainer;

namespace Content.StartWindow.Scripts.Controller
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.StartWindow)]
    public class StartWindowController : ControlEntity, IStartWindowController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ISceneStateMachine _sceneStateMachine;
        
        private readonly ILogManager _logger = new LogManager(nameof(StartWindowController));

        private MonoView _monoView;
        private Action _onStartButtonClicked;

        public bool IsInited { get; private set; }

        protected override async UniTask Load()
        {
            await _assetLoader.LoadAsync<GameObject>(ViewInfo.StartWindow);
        }
        
        protected override async UniTask Init()
        {
            await CreateView();
            
            if (!IsLoadSucceed())
            {
                _logger.LogError($"{ViewInfo.StartWindow} load failed.");
                return;
            }
            
            var isOnboardingCompleted = true;
            
            _onStartButtonClicked = isOnboardingCompleted
                ? _sceneStateMachine.EnterState<MetaSceneState>
                : _sceneStateMachine.EnterState<CoreSceneState>;

            RegisterAndInitView();
            AudioController.Instance.PlayMusic(MusicList.StartSceneMusic);
            IsInited = true;
        }

        private async UniTask CreateView()
        {
            _monoView = await _viewFactory.CreateView<MonoView>(ViewInfo.StartWindow, ViewType.Window);
        }

        private bool IsLoadSucceed()
        {
            var result = _monoView is not null;
            return result;
        }

        private void StartGame()
        {
            _onStartButtonClicked?.Invoke();
        }

        private void OpenSettings()
        {
            _viewManager.Open(ViewInfo.SettingsPopup);
        }

        private void OpenWebSite()
        {
            ExternalSourcesController.Instance.OpenPrivacyPolicy();
        }

        private void RegisterAndInitView()
        {
            var viewSignalManager = new ViewSignalManager()
                .AddSignal(StartWindowInfo.StartGameSignal, StartGame)
                .AddSignal(StartWindowInfo.OpenSettingsSignal, OpenSettings)
                .AddSignal(StartWindowInfo.OpenWebSiteSignal, OpenWebSite);
            
            new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.StartWindow)
                .SetViewType(ViewType.Window)
                .SetView(_monoView)
                .SetViewSignalManager(viewSignalManager)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}