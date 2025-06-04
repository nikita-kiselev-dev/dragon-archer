using Core.Logger;
using Core.Scene;

namespace Core.SceneStateMachine.SceneStates
{
    public class StartSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly ILogManager _logger = new LogManager(nameof(StartSceneState));

        public StartSceneState(ISceneService sceneService)
        {
            _sceneService = sceneService;

        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneConstants.StartScene, OnLoaded);
            _logger.Log("Load scene started.");
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            _logger.Log("Load scene completed.");
        }
    }
}