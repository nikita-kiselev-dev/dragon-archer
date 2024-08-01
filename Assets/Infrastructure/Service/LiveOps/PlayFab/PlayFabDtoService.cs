using System.Collections.Generic;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabDtoService
    {
        private readonly ISignalBus _signalBus;
        
        private Dictionary<string, string> _titleData;

        public Dictionary<string, string> TitleData => _titleData;

        public PlayFabDtoService(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void LoadTitleDataFromServer()
        {
            PlayFabClientAPI.GetTitleData(new GetTitleDataRequest(), OnLoadTitleDataSuccess, OnLoadTitleDataFailure);
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