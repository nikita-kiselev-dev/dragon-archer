using Content.DailyBonus.Scripts;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using VContainer;

namespace Infrastructure.Game.GameManager
{
    public class MetaGameManager : IMetaGameManager
    {
        private readonly IServerConnectionService _serverConnectionService;
        private readonly ISignalBus _signalBus;
        private readonly IDailyBonus _dailyBonus;

        [Inject]
        public MetaGameManager(
            IServerConnectionService serverConnectionService, 
            ISignalBus signalBus,
            IDailyBonus dailyBonus)
        {
            _serverConnectionService = serverConnectionService;
            _signalBus = signalBus;
            _dailyBonus = dailyBonus;
        }
        
        public void OnSceneStart()
        {
            if (_serverConnectionService.IsConnectedToServer)
            {
                _dailyBonus.Init();
            }
            
            _signalBus.Trigger<OnGameManagerStartedSignal>();
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }
    }
}