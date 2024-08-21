using System;
using System.Collections.Generic;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabService : ILiveOpsController, IServerConnectionService, IServerTimeService, IDtoService
    {
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private ISignalBus _signalBus;
        
        private PlayFabServerTimeService _timeService;
        private PlayFabLoginService _loginService;
        private PlayFabDtoService _dtoService;

        public bool IsConnectedToServer => _loginService.IsLoggedIn;
        public DateTime ServerTime => _timeService.GetServerTimeAsync();
        
        public void Init()
        {
            _timeService = new PlayFabServerTimeService(_coroutineRunner);
            _loginService = new PlayFabLoginService(_signalBus, LoadServices);
            _dtoService = new PlayFabDtoService(_signalBus);
            _loginService.Login();
        }

        public Dictionary<string, string> GetDto()
        {
            return _dtoService.TitleData;
        }

        private void LoadServices()
        {
            _dtoService.LoadTitleDataFromServer();
        }
    }
}