using System;
using Content.SettingsPopup.Scripts.Presenter;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Game.Tutorials;
using Infrastructure.Game.Tutorials.Data;
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
        [Inject] private readonly ITutorialService _tutorialService;
        [Inject] private readonly ISettingsPopupPresenter _settingsPopupPresenter;
        
        private IController _startScreenController;
        
        public void OnSceneStart()
        {
            _settingsPopupPresenter.Init();
            SetupStartScreen();
        }

        public void OnSceneExit()
        {
            Debug.Log("Start Scene Exit");
        }
        
        private void SetupStartScreen()
        {
            var onboardingCompleted = _tutorialService
                .IsTutorialCompleted<OnboardingTutorialData>();
            
            Action startAction = onboardingCompleted
                ? () => _sceneStateMachine.EnterState<MetaSceneState>()
                : () => _sceneStateMachine.EnterState<CoreSceneState>();
            
            _startScreenController = new StartScreenController(
                _viewFactory,
                _viewManager,
                startAction);
            
            _startScreenController.Init();
        }
    }
}