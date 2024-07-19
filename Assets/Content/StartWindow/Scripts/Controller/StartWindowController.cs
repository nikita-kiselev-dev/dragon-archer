using System;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.StartWindow.Scripts.Controller
{
    public class StartWindowController : IStartWindowController
    {
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;

        private readonly Action _openMetaScene;
        private readonly Action _openCoreScene;
        
        public StartWindowController(
            IViewFactory viewFactory,
            IViewManager viewManager,
            Action openMetaScene,
            Action openCoreScene)
        {
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _openMetaScene = openMetaScene;
            _openCoreScene = openCoreScene;
        }
        
        public void Init()
        {
            RegisterAndInitView();
            AudioService.Instance.PlayMusic(MusicInfo.StartWindowMusic);
        }

        private void StartGame()
        {
            //TODO: load from save
            var tutorialCompleted = false;

            if (tutorialCompleted)
            {
                _openMetaScene?.Invoke();
            }
            else
            {
                _openCoreScene?.Invoke();
            }
        }

        private void OpenSettings()
        {
            _viewManager.Open(ViewInfo.SettingsPopupKey);
        }

        private void OpenWebSite()
        {
            ExternalSourcesController.Instance.OpenPrivacyPolicy();
        }

        private void RegisterAndInitView()
        {
            var view = _viewFactory.CreateView<IView>(ViewInfo.StartWindowKey, ViewType.Window);
            
            var viewSignalManager = new ViewSignalManager()
                .AddSignal(StartWindowInfo.StartGameSignal, StartGame)
                .AddSignal(StartWindowInfo.OpenSettingsSignal, OpenSettings)
                .AddSignal(StartWindowInfo.OpenWebSiteSignal, OpenWebSite);
            
            new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.StartWindowKey)
                .SetViewType(ViewType.Window)
                .SetView(view)
                .SetViewSignalManager(viewSignalManager)
                .EnableFromStart()
                .RegisterAndInit();
        }
    }
}