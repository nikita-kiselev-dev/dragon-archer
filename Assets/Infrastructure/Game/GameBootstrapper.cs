using Infrastructure.Service.Scene;
using Infrastructure.Service.SceneStateMachine;
using Infrastructure.Service.SceneStateMachine.SceneStates;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.Game
{
    public class GameBootstrapper : IGameBootstrapper
    {
        [Inject] private readonly ISceneStateMachine _sceneStateMachine;

        public void EnterGame()
        {
            if (SceneManager.GetActiveScene().name == SceneInfo.BootstrapScene)
            {
                _sceneStateMachine.EnterState<StartSceneState>();
            }
        }
    }
}