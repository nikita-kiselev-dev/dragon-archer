﻿using System;
using Infrastructure.Service.SignalBus;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;

namespace Infrastructure.Service.LiveOps.PlayFab
{
    public class PlayFabLoginService
    {
        private readonly ISignalBus _signalBus;
        private readonly Action<bool> _onLoginCompleted;
        
        private bool _isLoggedIn;

        public PlayFabLoginService(ISignalBus signalBus, Action<bool> onLoginCompleted)
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
            
            PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnLoginFailure);
        }

        private void OnLoginSuccess(LoginResult result)
        {
            Debug.Log($"{GetType().Name} - login successful!\nWelcome, " + result.PlayFabId + "!");
            _isLoggedIn = true;
            GetPlayerProfile();
            _onLoginCompleted?.Invoke(true);
        }

        private void OnLoginFailure(PlayFabError error)
        {
            Debug.LogError($"{GetType().Name} - error logging in: " + error.GenerateErrorReport());
            _onLoginCompleted?.Invoke(false);
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