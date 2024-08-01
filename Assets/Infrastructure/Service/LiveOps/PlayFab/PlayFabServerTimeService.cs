using System;
using System.Collections;
using System.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabServerTimeService
    {
        private const int ServerTimeRequestDelayInSeconds = 300;

        private readonly ICoroutineRunner _coroutineRunner;
        
        private DateTime _cachedServerTime;
        private bool _isRequestingServerTime;
        private Coroutine _requestDelayCoroutine;

        public PlayFabServerTimeService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public async Task<DateTime> GetServerTimeAsync()
        {
            if (_isRequestingServerTime && _requestDelayCoroutine != null)
            {
                return _cachedServerTime;
            }

            _isRequestingServerTime = true;
            _cachedServerTime = await RequestServerTimeAsync();
            _isRequestingServerTime = false;

            _requestDelayCoroutine = _coroutineRunner.StartCoroutine(RequestDelayCoroutine());
            
            return _cachedServerTime;
        }

        private IEnumerator RequestDelayCoroutine()
        {
            yield return new WaitForSeconds(ServerTimeRequestDelayInSeconds);
        }
        
        private Task<DateTime> RequestServerTimeAsync()
        {
            var taskCompletionSource = new TaskCompletionSource<DateTime>();

            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                result => OnGetTimeSuccess(result, taskCompletionSource),
                error => OnGetTimeFailure(error, taskCompletionSource));

            return taskCompletionSource.Task;
        }

        private void OnGetTimeSuccess(GetTimeResult result, TaskCompletionSource<DateTime> taskCompletionSource)
        {
            _cachedServerTime = result.Time;
            Debug.Log($"{GetType().Name} - server time: " + _cachedServerTime);
            taskCompletionSource.SetResult(result.Time);
        }
        
        private void OnGetTimeFailure(PlayFabError error, TaskCompletionSource<DateTime> taskCompletionSource)
        {
            Debug.LogError($"{GetType().Name} - error getting server time: " + error.GenerateErrorReport());
            taskCompletionSource.SetException(new Exception(error.GenerateErrorReport()));
        }
    }
}