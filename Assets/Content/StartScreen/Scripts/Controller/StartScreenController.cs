using System;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.StartScreen.Scripts.Controller
{
    public class StartScreenController : IStartScreenController
    {
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;

        private readonly Action _onStartButtonClicked;
        
        public StartScreenController(
            IViewFactory viewFactory,
            IViewManager viewManager,
            Action onStartButtonClicked)
        {
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _onStartButtonClicked = onStartButtonClicked;
        }
        
        public void Init()
        {
            RegisterAndInitView();
            AudioService.Instance.PlayMusic(MusicList.StartSceneMusic);
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
            var view = _viewFactory.CreateView<IView>(ViewInfo.StartScreen, ViewType.Window);
            
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