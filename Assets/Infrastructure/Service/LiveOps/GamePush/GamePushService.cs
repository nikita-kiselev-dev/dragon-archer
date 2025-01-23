using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Service.LiveOps.GamePush
{
    public class GamePushService : IServerConnectionService, IServerTimeService, IDtoService
    {
        [Inject] private ISignalBus _signalBus;

        private readonly GamePushServerTimeService _timeService = new();
        private readonly ILogManager _logger = new LogManager(nameof(GamePushService));

        public Dictionary<string, string> GetDto() => null;

        public bool IsConnectedToServer
        {
            get
            {
                _logger.LogError("No method for server connection check! Return false by default.");
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