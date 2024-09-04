using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps.Signals;
using Infrastructure.Service.SignalBus;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabLoginService
    {
        private const int LoginTimeoutInSeconds = 5;
        
        private readonly ISignalBus _signalBus;
        private readonly Action _onLoginCompleted;
        
        private bool _isLoggedIn;

        public bool IsLoggedIn => _isLoggedIn;

        public PlayFabLoginService(ISignalBus signalBus, Action onLoginCompleted)
        {
            _signalBus = signalBus;
            _onLoginCompleted = onLoginCompleted;
        }

        public void Login()
        {
            var request = new LoginWithCustomIDRequest 
            { 
                CustomId = "GettingStartedGuide", 
                CreateAccount = false 
            };
            
            var cancellationTokenSource = new CancellationTokenSource();
            cancellationTokenSource.CancelAfterSlim(TimeSpan.FromSeconds(LoginTimeoutInSeconds));

            try
            {
                PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
            }
            catch (OperationCanceledException exception)
            {
                if (exception.CancellationToken == cancellationTokenSource.Token)
                {
                    Debug.LogError($"{GetType().Name}: login timeout! Wait time: {LoginTimeoutInSeconds} seconds.");
                }
            }
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log($"{GetType().Name} - login successful!\nWelcome, " + result.PlayFabId + "!");
            _isLoggedIn = true;
            _onLoginCompleted?.Invoke();
            GetPlayerProfile();
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError($"{GetType().Name} - error logging in: " + error.GenerateErrorReport());
            _signalBus.Trigger<ServerLoginCompletedSignal, bool>(false);
        }

        private void GetPlayerProfile()
        {
            if (!_isLoggedIn)
            {
                Debug.LogError($"{GetType().Name} - cannot get player profile, not logged in.");
                return;
            }

            var request = new GetPlayerProfileRequest
            {
                PlayFabId = PlayFabSettings.staticPlayer.PlayFabId
            };
            
            PlayFabClientAPI.GetPlayerProfile(request, OnGetPlayerProfileSuccess, OnGetPlayerProfileFailure);
        }

        private void OnGetPlayerProfileSuccess(GetPlayerProfileResult result)
        {
            Debug.Log($"{GetType().Name} - player profile received: " + result.PlayerProfile.DisplayName);
            _signalBus.Trigger<ServerLoginCompletedSignal, bool>(true);
        }

        private void OnGetPlayerProfileFailure(PlayFabError error)
        {
            Debug.LogError($"{GetType().Name} - error getting player profile: " + error.GenerateErrorReport());
            _signalBus.Trigger<ServerLoginCompletedSignal, bool>(false);
        }
    }
}