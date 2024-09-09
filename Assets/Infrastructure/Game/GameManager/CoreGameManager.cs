using Infrastructure.Service.Audio;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class CoreGameManager : ICoreGameManager
    {
        private readonly ISignalBus _signalBus;

        [Inject]
        public CoreGameManager(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void OnSceneStart()
        {
            AudioController.Instance.PlayMusic(MusicList.CoreSceneMusic);
            _signalBus.Trigger<OnGameManagerStartedSignal>();
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}