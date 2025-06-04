using Core.DailyBonus.Scripts;
using Core.Logger;
using Core.Scene;

namespace Core.SceneStateMachine.SceneStates
{
    public class MetaSceneState : ISceneState
    {
        private readonly ISceneService _sceneService;
        private readonly ILogManager _logger = new LogManager(nameof(MetaSceneState));
        private readonly IDailyBonus _dailyBonus;

        public MetaSceneState(ISceneService sceneService)
        {
            _sceneService = sceneService;
        }

        public void Enter()
        {
            _sceneService.LoadScene(SceneConstants.MetaScene, OnLoaded);
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