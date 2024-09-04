using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Game.Data
{
    public class MainDataManager : IMainDataManager, IDisposable
    {
        private readonly MainData _mainData;
        private readonly IServerTimeService _serverTimeService;
        private readonly ISignalBus _signalBus;
        
        public DateTime LastSessionServerTime => _mainData.LastSessionServerTime;

        [Inject]
        public MainDataManager(MainData mainData, IServerTimeService serverTimeService, ISignalBus signalBus)
        {
            _mainData = mainData;
            _serverTimeService = serverTimeService;
            _signalBus = signalBus;
            
            _signalBus.Subscribe<GetServerTimeCompletedSignal, DateTime>(this, SetServerTime);
        }
        
        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<GetServerTimeCompletedSignal>(this);
        }

        public async UniTask<bool> IsFirstServerLaunch()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var isFirstLaunch = serverTime.Date == _mainData.FirstSessionServerTime.Date;
            return isFirstLaunch;
        }

        public void SetLocalTime(DateTime time)
        {
            _mainData.SetLocalTime(time);
        }

        public void SetServerTime(DateTime time)
        {
            _mainData.SetServerTime(time);
        }
    }
}