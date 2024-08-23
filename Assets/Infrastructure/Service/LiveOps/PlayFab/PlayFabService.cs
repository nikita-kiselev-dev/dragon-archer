using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.SignalBus;
using PlayFab;
using VContainer;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabService : ILiveOpsController, IServerConnectionService, IServerTimeService, IDtoService
    {
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private ISignalBus _signalBus;

        private readonly PlayFabServerTimeService _timeService = new();
        private PlayFabLoginService _loginService;
        private PlayFabDtoService _dtoService;

        public bool IsConnectedToServer => _loginService.IsLoggedIn;
        
        public void Init()
        {
            _loginService = new PlayFabLoginService(_signalBus, GetLiveOps);
            _dtoService = new PlayFabDtoService(_signalBus);
            _loginService.Login();
        }

        public Dictionary<string, string> GetDto()
        {
            return _dtoService.TitleData;
        }

        public async UniTask<DateTime> GetServerTime()
        {
            var serverTime = await _timeService.GetServerTimeAsync();
            return serverTime;
        }

        private void GetLiveOps()
        {
            if (!PlayFabClientAPI.IsClientLoggedIn())
            {
                return;
            }
            
            _timeService.GetServerTimeAsync();
            _dtoService.GetTitleDataFromServer();
        }
    }
}