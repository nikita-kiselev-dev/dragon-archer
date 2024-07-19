using Infrastructure.Game.GameManager;
using Infrastructure.Service.Scene;
using UnityEngine;

namespace Infrastructure.Service.StateMachine.SceneStates
{
    public class MetaSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly IGameManager _gameManager;

        public MetaSceneState(ISceneService sceneService, IGameManager gameManager)
        {
            _sceneService = sceneService;
            _gameManager = gameManager;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneInfo.MetaScene, OnLoaded);
        }

        public void Exit()
        {
            _gameManager.OnSceneExit();
            Debug.Log("Exit Meta Scene State");
        }

        private void OnLoaded()
        {
            _gameManager.OnSceneStart();
            Debug.Log("Entered Meta Scene State");
        }
    }
}