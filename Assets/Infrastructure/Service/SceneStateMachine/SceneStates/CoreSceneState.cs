using Infrastructure.Service.Audio;
using Infrastructure.Service.Logger;
using Infrastructure.Service.Scene;

namespace Infrastructure.Service.SceneStateMachine.SceneStates
{
    public class CoreSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly ILogManager _logger = new LogManager(nameof(CoreSceneState));

        public CoreSceneState(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneInfo.CoreScene, OnLoaded);
            _logger.Log("Load scene started.");
        }

        public void Exit()
        {
        }

        private void OnLoaded()
        {
            _logger.Log("Load scene completed.");
            AudioController.Instance.PlayMusic(MusicList.CoreSceneMusic);
        }
    }
}