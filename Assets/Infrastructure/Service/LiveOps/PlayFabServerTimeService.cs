using System;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps
{
    public class PlayFabServerTimeService
    {
        private readonly ICoroutineRunner _coroutineRunner;
        
        private const int ServerTimeRequestDelayInSeconds = 300;
        
        private DateTime _cachedServerTime;
        private Coroutine _serverTimeRequestCoroutine;

        public PlayFabServerTimeService(ICoroutineRunner coroutineRunner)
        {
            _coroutineRunner = coroutineRunner;
        }
        
        public void GetServerTime(Action<DateTime> callback = null)
        {
            if (_serverTimeRequestCoroutine == null)
            {
                _serverTimeRequestCoroutine = _coroutineRunner.StartCoroutine(ServerTimeCoroutineDelay());
            }
            else
            {
                callback?.Invoke(_cachedServerTime);
            }
        }
        
        private IEnumerator ServerTimeCoroutineDelay(Action<DateTime> callback = null)
        {
            RequestServerTime(callback);
            yield return new WaitForSeconds(ServerTimeRequestDelayInSeconds);
        }

        private void RequestServerTime(Action<DateTime> callback = null)
        {
            PlayFabClientAPI.GetTime(
                new GetTimeRequest(), 
                result => OnGetTimeSuccess(result, callback),
                OnGetTimeFailure);
        }

        private void OnGetTimeSuccess(GetTimeResult result, Action<DateTime> callback)
        {
            _cachedServerTime = result.Time;
            Debug.Log($"{GetType().Name} - server time: " + _cachedServerTime);
            callback?.Invoke(_cachedServerTime);
        }
        
        private void OnGetTimeFailure(PlayFabError error)
        {
            Debug.LogError($"{GetType().Name} - error getting server time: " + error.GenerateErrorReport());
        }
    }
}