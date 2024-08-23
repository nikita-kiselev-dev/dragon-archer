using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabDtoService
    {
        private const int ServerTimeoutInSeconds = 5;
        private readonly ISignalBus _signalBus;
        private Dictionary<string, string> _titleData;

        public Dictionary<string, string> TitleData => _titleData;

        public PlayFabDtoService(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void GetTitleDataFromServer()
        {
            var completionSource = new UniTaskCompletionSource<Dictionary<string, string>>();
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(ServerTimeoutInSeconds));
            
            try
            {
                PlayFabClientAPI.GetTitleData(
                    new GetTitleDataRequest(),
                    OnLoadTitleDataSuccess,
                    OnLoadTitleDataFailure);
            }
            catch (OperationCanceledException exception)
            {
                if (exception.CancellationToken == cancellationTokenSource.Token)
                {
                    completionSource.TrySetException(new Exception(exception.ToString()));
                }
            }
        }

        private void OnLoadTitleDataSuccess(GetTitleDataResult result)
        {
            if (result.Data == null)
            {
                Debug.Log($"{GetType().Name}: loading success, but server config is null!");
            }
            else
            {
                _titleData = result.Data;
                _signalBus.Trigger<GetLiveOpsDataCompletedSignal>();
                Debug.Log($"{GetType().Name}: loading success, title data saved!");
            }
        }

        private void OnLoadTitleDataFailure(PlayFabError error)
        {
            Debug.Log($"{GetType().Name}: loading failed: {error.GenerateErrorReport()}");
        }
    }
}