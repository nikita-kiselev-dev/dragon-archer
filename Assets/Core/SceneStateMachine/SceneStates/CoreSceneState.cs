using Core.Audio;
using Core.Logger;
using Core.Scene;

namespace Core.SceneStateMachine.SceneStates
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
            _sceneService.LoadScene(SceneConstants.CoreScene, OnLoaded);
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