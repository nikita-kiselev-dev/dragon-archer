using Infrastructure.Service.Logger;
using Infrastructure.Service.SignalBus.Monobehavior;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.SignalBus.Monobehaviour
{
    public class MonoBehaviourStatusHandler : MonoBehaviour
    {
        [Inject] private readonly ISignalBus _signalBus;

        private readonly ILogManager _logger = new LogManager(nameof(MonoBehaviourStatusHandler));

        private void Awake()
        {
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
    }
}