using Infrastructure.Service.Initialization.Signals;
using Infrastructure.Service.Scene;
using Infrastructure.Service.SceneStateMachine;
using Infrastructure.Service.SceneStateMachine.SceneStates;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.Game
{
    public class GameBootstrapper : MonoBehaviour, IGameBootstrapper
    {
        private ISceneStateMachine _sceneStateMachine;
        private ISignalBus _signalBus;

        [Inject]
        private void Init(ISceneStateMachine sceneStateMachine, ISignalBus signalBus)
        {
            _sceneStateMachine = sceneStateMachine;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnSceneInitCompletedSignal>(this, EnterGame);
        }

        private void EnterGame()
        {
            if (SceneManager.GetActiveScene().name == SceneConstants.BootstrapScene)
            {
                _sceneStateMachine.EnterState<StartSceneState>();
            }
        }

        private void OnDestroy()
        {
            _signalBus?.Unsubscribe<OnSceneInitCompletedSignal>(this);
        }
    }
}