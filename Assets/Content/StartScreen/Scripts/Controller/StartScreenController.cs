using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.StateMachine.SceneStates;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.StartScreen.Scripts.Controller
{
    public class StartScreenController : IStartScreenController
    {
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;
        private readonly IStateMachine _sceneStateMachine;

        private readonly Action _onStartButtonClicked;

        public bool IsInited { get; private set; }
        
        public StartScreenController(
            IViewFactory viewFactory,
            IViewManager viewManager,
            IStateMachine sceneStateMachine)
        {
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            
            var isOnboardingCompleted = true;
            _onStartButtonClicked = isOnboardingCompleted
                ? sceneStateMachine.EnterState<MetaSceneState>
                : sceneStateMachine.EnterState<CoreSceneState>;
        }
        
        public void Init()
        {
            RegisterAndInitView().Forget();
            AudioController.Instance.PlayMusic(MusicList.StartSceneMusic);
            IsInited = true;
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

        private async UniTaskVoid RegisterAndInitView()
        {
            var view = await _viewFactory.CreateView<IView>(ViewInfo.StartScreen, ViewType.Window);
            
            var viewSignalManager = new ViewSignalManager()
                .AddSignal(StartScreenInfo.StartGameSignal, StartGame)
                .AddSignal(StartScreenInfo.OpenSettingsSignal, OpenSettings)
                .AddSignal(StartScreenInfo.OpenWebSiteSignal, OpenWebSite);
            
            new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.StartScreen)
                .SetViewType(ViewType.Window)
                .SetView(view)
                .SetViewSignalManager(viewSignalManager)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}