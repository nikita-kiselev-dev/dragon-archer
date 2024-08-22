using System;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabServerTimeService
    {
        private const int ServerTimeRequestDelayInSeconds = 300;
        
        private DateTime _cachedServerTime;
        private bool _isRequestingServerTime;
        private Coroutine _requestDelayCoroutine;
        private bool _isRequestDelayEnabled;
        
        public async UniTask<DateTime> GetServerTimeAsync()
        {
            if (_isRequestingServerTime || _isRequestDelayEnabled)
            {
                return _cachedServerTime;
            }

            _isRequestingServerTime = true;
            _cachedServerTime = await RequestServerTimeAsync();
            _isRequestingServerTime = false;

            if (!_isRequestDelayEnabled)
            {
                await WaitRequestDelay();
            }
            
            return _cachedServerTime;
        }

        private async UniTask WaitRequestDelay()
        { 
            _isRequestDelayEnabled = true;
            await UniTask.WaitForSeconds(ServerTimeRequestDelayInSeconds);
            _isRequestDelayEnabled = false;
        }
        
        private UniTask<DateTime> RequestServerTimeAsync()
        { 
            var uniTaskCompletionSource = new UniTaskCompletionSource<DateTime>();
            
            PlayFabClientAPI.GetTime(new GetTimeRequest(),
                result => OnGetTimeSuccess(result, uniTaskCompletionSource),
                error => OnGetTimeFailure(error, uniTaskCompletionSource));
            
            return uniTaskCompletionSource.Task;
        }

        private void OnGetTimeSuccess(GetTimeResult result, IResolvePromise<DateTime> uniTaskCompletionSource)
        {
            _cachedServerTime = result.Time;
            Debug.Log($"{GetType().Name} - server time: " + _cachedServerTime);
            uniTaskCompletionSource.TrySetResult(result.Time);
        }
        
        private void OnGetTimeFailure(PlayFabError error, IRejectPromise uniTaskCompletionSource)
        {
            Debug.LogError($"{GetType().Name} - error getting server time: " + error.GenerateErrorReport());
            uniTaskCompletionSource.TrySetException(new Exception(error.GenerateErrorReport()));
        }
    }
}