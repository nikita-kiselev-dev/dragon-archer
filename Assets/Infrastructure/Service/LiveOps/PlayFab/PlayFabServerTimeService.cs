using System;
using System.Collections;
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
        
        public DateTime GetServerTimeAsync()
        {
            if (_isRequestingServerTime && _requestDelayCoroutine != null)
            {
                return _cachedServerTime;
            }

            _isRequestingServerTime = true;
            _cachedServerTime = RequestServerTimeAsync();
            _isRequestingServerTime = false;

            _requestDelayCoroutine = _coroutineRunner.StartCoroutine(RequestDelayCoroutine());
            
            return _cachedServerTime;
        }

        private IEnumerator RequestDelayCoroutine()
        {
            yield return new WaitForSeconds(ServerTimeRequestDelayInSeconds);
        }
        
        private DateTime RequestServerTimeAsync()
        { 
            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                result => OnGetTimeSuccess(result),
                error => OnGetTimeFailure(error));

            return DateTime.Now;
        }

        private void OnGetTimeSuccess(GetTimeResult result)
        {
            _cachedServerTime = result.Time;
            Debug.Log($"{GetType().Name} - server time: " + _cachedServerTime);
        }
        
        private void OnGetTimeFailure(PlayFabError error)
        {
            Debug.LogError($"{GetType().Name} - error getting server time: " + error.GenerateErrorReport());
        }
    }
}