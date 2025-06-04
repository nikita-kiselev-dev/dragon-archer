using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Core.LiveOps.PlayFab
{
    public class PlayFabServerTimeService
    {
        private const int ServerTimeRequestDelayInSeconds = 300;
        private const int ServerTimeoutInSeconds = 5;
        
        private DateTime _cachedServerTime;
        private bool _isRequestingServerTime;
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

            if (_isRequestDelayEnabled)
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
            var completionSource = new UniTaskCompletionSource<DateTime>();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(ServerTimeoutInSeconds));

            try
            {
                PlayFabClientAPI.GetTime(new GetTimeRequest(),
                    result => OnGetTimeSuccess(result, completionSource),
                    error => OnGetTimeFailure(error, completionSource));
            }
            catch (OperationCanceledException exception)
            {
                if (exception.CancellationToken == cancellationTokenSource.Token)
                {
                    completionSource.TrySetException(new Exception(exception.ToString()));
                }
            }
            
            return completionSource.Task;
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