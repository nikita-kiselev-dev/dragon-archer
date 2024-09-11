using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.LiveOps.GamePush
{
    public class GamePushService : IServerConnectionService, IServerTimeService
    {
        [Inject] private ISignalBus _signalBus;

        private readonly GamePushServerTimeService _timeService = new();

        public bool IsConnectedToServer
        {
            get
            {
                Debug.LogWarning($"{GetType().Name}: no method for server connection check! Return true by default.");
                return true;
            }
        }

        public async UniTask<DateTime> GetServerTime()
        {
            await UniTask.WaitUntil(() => true);
            var serverTime = _timeService.GetServerTime();
            _signalBus.Trigger<GetServerTimeCompletedSignal, DateTime>(serverTime);
            return serverTime;
        }
    }
}