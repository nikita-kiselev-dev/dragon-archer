using System;
using System.Collections.Generic;
using Core.LiveOps.Signals;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using GamePush;
using VContainer;

namespace Core.LiveOps.GamePush
{
    public class GamePushService : IServerConnectionService, IServerTimeService, IDtoService
    {
        [Inject] private ISignalBus _signalBus;

        public Dictionary<string, string> GetDto() => null;

        public bool IsConnectedToServer => true;

        public UniTask<DateTime> GetServerTime()
        {
            var serverTime = GP_Server.Time();
            _signalBus.Trigger<GetServerTimeCompletedSignal, DateTime>(serverTime);
            return new UniTask<DateTime>(serverTime);
        }
    }
}