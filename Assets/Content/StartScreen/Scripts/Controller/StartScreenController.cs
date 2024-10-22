using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Scopes;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.StateMachine.SceneStates;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;
using VContainer;

namespace Content.StartScreen.Scripts.Controller
{
    [ControlEntityOrder(nameof(BootstrapScope), (int)StartSceneInitOrder.StartScreen)]
    public class StartScreenController : ControlEntity, IStartScreenController
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IStateMachine _sceneStateMachine;

        private IView _view;
        private Action _onStartButtonClicked;

        public bool IsInited { get; private set; }

        protected override async UniTask Load()
        {
            _view = await _viewFactory.CreateView<IView>(ViewInfo.StartScreen, ViewType.Window);
        }
        
        protected override UniTask Init()
        {
            var isOnboardingCompleted = true;
            
            _onStartButtonClicked = isOnboardingCompleted
                ? _sceneStateMachine.EnterState<MetaSceneState>
                : _sceneStateMachine.EnterState<CoreSceneState>;

            RegisterAndInitView();
            AudioController.Instance.PlayMusic(MusicList.StartSceneMusic);
            IsInited = true;

            return UniTask.CompletedTask;
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
                .AddSignal(StartScreenInfo.StartGameSignal, StartGame)
                .AddSignal(StartScreenInfo.OpenSettingsSignal, OpenSettings)
                .AddSignal(StartScreenInfo.OpenWebSiteSignal, OpenWebSite);
            
            new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.StartScreen)
                .SetViewType(ViewType.Window)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}