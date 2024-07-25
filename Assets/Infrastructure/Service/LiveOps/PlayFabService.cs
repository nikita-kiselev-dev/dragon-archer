using System;
using System.Threading.Tasks;
using Infrastructure.Service.SignalBus;
using VContainer;

namespace Infrastructure.Service.LiveOps
{
    public class PlayFabService : ILiveOpsController, IServerTimeService
    {
        [Inject] private ICoroutineRunner _coroutineRunner;
        [Inject] private ISignalBus _signalBus;

        private PlayFabServerTimeService _timeService;
        private PlayFabLoginService _loginService;
        
        public void Init()
        {
            _timeService = new PlayFabServerTimeService(_coroutineRunner);
            
            _loginService = new PlayFabLoginService(_signalBus);
            _loginService.Login();
        }

        public async Task<DateTime> GetServerTime()
        {
            return await _timeService.GetServerTimeAsync();
        }
    }
}