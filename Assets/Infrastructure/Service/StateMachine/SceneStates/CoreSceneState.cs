using Infrastructure.Game.GameManager;
using Infrastructure.Service.Scene;
using UnityEngine;

namespace Infrastructure.Service.StateMachine.SceneStates
{
    public class CoreSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly IGameManager _gameManager;

        public CoreSceneState(ISceneService sceneService, IGameManager gameManager)
        {
            _sceneService = sceneService;
            _gameManager = gameManager;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneInfo.CoreScene, OnLoaded);
        }

        public void Exit()
        {
            _gameManager.OnSceneExit();
            Debug.Log("Exit Core Scene State");
        }

        private void OnLoaded()
        {
            _gameManager.OnSceneStart();
            Debug.Log("Entered Core Scene State");
        }
    }
}