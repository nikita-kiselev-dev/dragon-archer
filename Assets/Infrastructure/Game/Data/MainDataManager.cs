using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Game.Data
{
    public class MainDataManager : IMainDataManager
    {
        private readonly MainData _mainData;
        private readonly IServerTimeService _serverTimeService;

        public DateTime LastSessionServerTime => _mainData.LastSessionServerTime;

        [Inject]
        public MainDataManager(MainData mainData, IServerTimeService serverTimeService, ISignalBus signalBus)
        {
            _mainData = mainData;
            _serverTimeService = serverTimeService;

            signalBus.Subscribe<GetServerTimeCompletedSignal, DateTime>(this, SetServerTime);
        }

        public bool IsFirstServerLaunch()
        {
            return _mainData.IsFirstServerLaunch;
        }

        public void SetLocalTime(DateTime time)
        {
            _mainData.SetLocalTime(time);
        }

        private void SetServerTime(DateTime time)
        {
            _mainData.SetServerTime(time);
        }
    }
}