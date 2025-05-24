using System;
using Content.SettingsPopup.Scripts;
using Content.StartWindow.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Game;
using Infrastructure.Service.Audio;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.Decorators.FastView;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.SceneStateMachine;
using Infrastructure.Service.SceneStateMachine.SceneStates;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.StartWindow.Scripts.Controller
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