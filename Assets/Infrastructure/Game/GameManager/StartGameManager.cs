using System;
using Content.Settings.Scripts;
using Content.StartScreen.Scripts.Controller;
using Infrastructure.Game.Tutorials;
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
        [Inject] private readonly ISettingsPopup _settingsPopup;
        
        private IStartScreenController _startScreenController;
        
        public void OnSceneStart()
        {
            _settingsPopup.Init();
            SetupStartScreen();
            
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
        
        private void SetupStartScreen()
        {
            /*var onboardingCompleted = _tutorialService
                .IsTutorialCompleted<OnboardingTutorialData>();*/

            var isOnboardingCompleted = true;
            
            Action startAction = isOnboardingCompleted
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