using System;
using System.Collections.Generic;
using System.Threading;
using Core.LiveOps.Signals;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Core.LiveOps.PlayFab
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
                    Debug.LogError(exception);
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