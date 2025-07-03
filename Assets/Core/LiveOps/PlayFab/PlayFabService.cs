using System;
using System.Collections.Generic;
using Core.LiveOps.Signals;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using PlayFab;

namespace Core.LiveOps.PlayFab
{
    public class PlayFabService : IServerConnectionService, IServerTimeService, IDtoService
    {
        private readonly ISignalBus _signalBus;
        private readonly PlayFabServerTimeService _timeService = new();
        
        private PlayFabLoginService _loginService;
        private PlayFabDtoService _dtoService;

        public bool IsConnectedToServer => _loginService.IsLoggedIn;
        
        public PlayFabService(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            _loginService = new PlayFabLoginService(_signalBus, GetLiveOps);
            _dtoService = new PlayFabDtoService(_signalBus);
            _loginService.Login();
        }

        public Dictionary<string, string> GetDto() => _dtoService.TitleData;

        public async UniTask<DateTime> GetServerTime()
        {
            var serverTime = await _timeService.GetServerTimeAsync();
            _signalBus.Trigger<GetServerTimeCompletedSignal, DateTime>(serverTime);
            return serverTime;
        }

        private void GetLiveOps()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                return;
            }
            
            //_timeService.GetServerTimeAsync().Forget();
            _dtoService.GetTitleDataFromServer();
        }
    }
}