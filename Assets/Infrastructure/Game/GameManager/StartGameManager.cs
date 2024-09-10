using System.Collections.Generic;
using Content.Settings.Scripts;
using Content.StartScreen.Scripts.Controller;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Tutorials;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.StateMachine;
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
        [Inject] private readonly ISignalBus _signalBus;
        
        private IStartScreenController _startScreenController;
        
        public async void OnSceneStart()
        {
            _settingsPopup.Init();
            ConfigureStartScreen();
            await WaitForInit();
            
            _signalBus.Trigger<OnGameManagerStartedSignal>();
            
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
        
        private void ConfigureStartScreen()
        {
            _startScreenController = new StartScreenController(
                _viewFactory,
                _viewManager,
                _sceneStateMachine);
            
            _startScreenController.Init();
        }

        private async UniTask WaitForInit()
        {
            var initOperations = new List<UniTask>
            {
                UniTask.WaitUntil(() => _settingsPopup.IsInited),
                UniTask.WaitUntil(() => _startScreenController.IsInited)
            };

            await UniTask.WhenAll(initOperations);
        }
    }
}