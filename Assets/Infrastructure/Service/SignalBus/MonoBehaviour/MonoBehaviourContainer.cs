using Infrastructure.Service.Logger;
using Infrastructure.Service.Scene.Signals;
using VContainer;

namespace Infrastructure.Service.SignalBus.MonoBehaviour
{
    public class MonoBehaviourContainer : UnityEngine.MonoBehaviour
    {
        [Inject] private readonly ISignalBus _signalBus;

        private readonly ILogManager _logger = new LogManager(nameof(MonoBehaviourContainer));
        
        public static MonoBehaviourContainer Instance { get; private set; }

        private void StopAllMonoCoroutines()
        {
            StopAllCoroutines();
        }

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
            Subscribe();
            _signalBus.Trigger<OnAwakeSignal>();
            _logger.Log("Awake.");
        }

        private void Start()
        {
            _signalBus.Trigger<OnStartSignal>();
            _logger.Log("Start.");
        }

        private void OnApplicationQuit()
        {
            _signalBus.Trigger<OnApplicationQuitSignal>();
            _logger.Log("ApplicationQuit.");
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            _signalBus.Trigger<OnApplicationFocusSignal, bool>(hasFocus);
            _logger.Log($"ApplicationFocus ({hasFocus}).");
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            _signalBus.Trigger<OnApplicationPauseSignal, bool>(pauseStatus);
            _logger.Log($"ApplicationPause ({pauseStatus}).");
        }

        private void OnDestroy()
        {
            Unsubscribe();
        }

        private void Subscribe()
        {
            _signalBus.Subscribe<StartSceneChangeSignal>(this, StopAllMonoCoroutines);
        }

        private void Unsubscribe()
        {
            _signalBus?.Unsubscribe<StartSceneChangeSignal>(this);
        }
    }
}