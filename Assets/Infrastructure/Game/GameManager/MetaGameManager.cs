using System.Collections.Generic;
using Content.DailyBonus.Scripts;
using Cysharp.Threading.Tasks;
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
        
        public async void OnSceneStart()
        {
            if (_serverConnectionService.IsConnectedToServer)
            {
                await StartWithServerConnection();
            }
            
            _signalBus.Trigger<OnGameManagerStartedSignal>();
            
            Debug.Log($"{GetType().Name}: start");
        }

        public void OnSceneExit()
        {
            Debug.Log($"{GetType().Name}: exit");
        }

        private async UniTask StartWithServerConnection()
        {
            _dailyBonus.Init();
            await WaitForInit();
        }

        private async UniTask WaitForInit()
        {
            var initOperations = new List<UniTask>
            {
                UniTask.WaitUntil(() => _dailyBonus.IsInited)
            };

            await UniTask.WhenAll(initOperations);
        }
    }
}