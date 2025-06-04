using System;
using Core.Audio;
using Core.Initialization.Scripts;
using Core.Initialization.Scripts.Decorators.FastView;
using Core.Initialization.Scripts.InitOrder;
using Core.Initialization.Scripts.Scopes;
using Core.SceneStateMachine;
using Core.SceneStateMachine.SceneStates;
using Core.SettingsPopup.Scripts;
using Core.StartWindow.Scripts.View;
using Core.Utilities.Scripts;
using Core.View.ViewManager;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.StartWindow.Scripts.Controller
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.StartWindow)]
    [FastViewDecoratable]
    public class StartWindowController : ControlEntity, IStartWindowController
    {
        [Inject] private readonly ISceneStateMachine _sceneStateMachine;
        [Inject] private readonly ISettings _settings;

        [FastView(StartWindowConstants.WindowKey, ViewType.Window)] private StartWindowView _view;
        private Action _onStartButtonClicked;
        private bool _isInited;

        bool IStartWindowController.IsInited => _isInited;
        
        protected override UniTask Init()
        {
            var isOnboardingCompleted = true;
            ConfigureView();
            
            _onStartButtonClicked = isOnboardingCompleted
                ? _sceneStateMachine.EnterState<MetaSceneState>
                : _sceneStateMachine.EnterState<CoreSceneState>;
            
            AudioController.Instance.PlayMusic(MusicList.StartSceneMusic);
            _isInited = true;
            return UniTask.CompletedTask;
        }

        private void ConfigureView()
        {
            _view.Init(StartGame, OpenSettings, OpenWebSite);
        }

        private void StartGame()
        {
            _onStartButtonClicked?.Invoke();
        }

        private void OpenSettings()
        {
            _settings.OpenPopup();
        }

        private void OpenWebSite()
        {
            ExternalSourcesController.Instance.OpenPrivacyPolicy();
        }
    }
}