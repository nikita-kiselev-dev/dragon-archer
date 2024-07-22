using Content.SettingsPopup.Scripts.Presenter;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Service;
using Infrastructure.Service.StateMachine;
using Infrastructure.Service.StateMachine.SceneStates;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class StartGameManager : IStartGameManager
    {
        [Inject] private readonly IStateMachine _sceneStateMachine;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly ISettingsPopupPresenter _settingsPopupPresenter;
        
        private IController _startWindowController;
        
        public void OnSceneStart()
        {
            _settingsPopupPresenter.Init();
            SetupStartWindow();
        }

        public void OnSceneExit()
        {
            Debug.Log("Start Scene Exit");
        }
        
        private void SetupStartWindow()
        {
            _startWindowController = new StartScreenController(
                _viewFactory,
                _viewManager,
                () => _sceneStateMachine.EnterState<MetaSceneState>(),
                () => _sceneStateMachine.EnterState<CoreSceneState>());
            
            _startWindowController.Init();
        }
    }
}