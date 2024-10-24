using Infrastructure.Service.Logger;
using Infrastructure.Service.Scene;

namespace Infrastructure.Service.StateMachine.SceneStates
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
            _sceneService.LoadScene(SceneInfo.StartScene, OnLoaded);
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