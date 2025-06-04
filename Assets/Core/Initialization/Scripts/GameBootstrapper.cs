using Core.Initialization.Scripts.Signals;
using Core.Scene;
using Core.SceneStateMachine;
using Core.SceneStateMachine.SceneStates;
using Core.SignalBus;
using VContainer;

namespace Core.Initialization.Scripts
{
    public class GameBootstrapper : UnityEngine.MonoBehaviour, IGameBootstrapper
    {
        private ISceneStateMachine _sceneStateMachine;
        private ISignalBus _signalBus;

        [Inject]
        private void Init(ISceneStateMachine sceneStateMachine, ISignalBus signalBus)
        {
            _sceneStateMachine = sceneStateMachine;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<OnSceneInitCompletedSignal, string>(this, EnterGame);
        }

        private void EnterGame(string sceneName)
        {
            if (sceneName == SceneConstants.BootstrapScene)
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