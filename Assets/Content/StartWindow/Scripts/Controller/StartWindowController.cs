using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Logger;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.StateMachine.SceneStates;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using VContainer;

namespace Content.StartWindow.Scripts.Controller
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.StartWindow)]
    public class StartWindowController : ControlEntity, IStartWindowController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IStateMachine _sceneStateMachine;
        
        private readonly ILogManager _logger = new LogManager(nameof(StartWindowController));

        private IView _view;
        private Action _onStartButtonClicked;

        public bool IsInited { get; private set; }

        protected override async UniTask Load()
        {
            _view = await _viewFactory.CreateView<IView>(ViewInfo.StartWindow, ViewType.Window);
        }
        
        protected override UniTask Init()
        {
            if (!IsLoadSucceed())
            {
                _logger.LogError($"{ViewInfo.StartWindow} load failed.");
                return UniTask.CompletedTask;
            }
            
            var isOnboardingCompleted = true;
            
            _onStartButtonClicked = isOnboardingCompleted
                ? _sceneStateMachine.EnterState<MetaSceneState>
                : _sceneStateMachine.EnterState<CoreSceneState>;

            RegisterAndInitView();
            AudioController.Instance.PlayMusic(MusicList.StartSceneMusic);
            IsInited = true;

            return UniTask.CompletedTask;
        }

        private bool IsLoadSucceed()
        {
            var result = _view is not null;
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
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}