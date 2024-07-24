using Infrastructure.Game.GameManager;
using Infrastructure.Service.Scene;

namespace Infrastructure.Service.StateMachine.SceneStates
{
    public class StartSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly IGameManager _gameManager;

        public StartSceneState(ISceneService sceneService, IGameManager gameManager)
        {
            _sceneService = sceneService;
            _gameManager = gameManager;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneInfo.StartScene, OnLoaded);
        }

        public void Exit()
        {
            _gameManager.OnSceneExit();
        }

        private void OnLoaded()
        {
            _gameManager.OnSceneStart();
        }
    }
}