using System;
using Infrastructure.Service.Scene;
using UnityEngine;

namespace Infrastructure.Service.StateMachine.SceneStates
{
    public class BootstrapSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly Action _onSceneLoaded;

        public BootstrapSceneState(ISceneService sceneService, Action onSceneLoaded)
        {
            _sceneService = sceneService;
            _onSceneLoaded = onSceneLoaded;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneInfo.BootstrapScene, OnLoaded);
        }

        public void Exit()
        {
            Debug.Log("Exit Bootstrap Scene State");
        }

        private void OnLoaded()
        {
            Debug.Log("Entered Bootstrap Scene State");
            _onSceneLoaded?.Invoke();
        }
    }
}