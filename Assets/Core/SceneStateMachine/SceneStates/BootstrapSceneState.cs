using System;
using Core.Scene;

namespace Core.SceneStateMachine.SceneStates
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
            _sceneService.LoadScene(SceneConstants.BootstrapScene, OnLoaded);
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