﻿using System;
using Infrastructure.Service.Scene;

namespace Infrastructure.Service.SceneStateMachine.SceneStates
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
        }

        private void OnLoaded()
        {
            _onSceneLoaded?.Invoke();
        }
    }
}