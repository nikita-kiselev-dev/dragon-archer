﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;

namespace Infrastructure.Game
{
    public class GameBootstrapperQ : IAsyncStartable, IDisposable
    {
        //[Inject] private readonly ISignalBus _signalBus;
        //[Inject] private readonly ISaveLoadService _saveLoadService;
        //[Inject] private readonly IDtoManager _dtoManager;

        //[Inject] private readonly ILiveOpsController _liveOpsController;
        //[Inject] private readonly IGame _game;

        private bool _isLocalizationReady;
        private bool _isServerLoginCompleted;
        
        async UniTask IAsyncStartable.StartAsync(CancellationToken cancellation)
        {
           // _signalBus.Subscribe<SaveFileLoadCompletedSignal>(this, InitServer);
           // _signalBus.Subscribe<ServerLoginCompletedSignal, bool>(this, InitGame);
           // _saveLoadService.Init();
            //_dtoManager.Init();
        }

        void IDisposable.Dispose()
        {
            //_signalBus?.Unsubscribe<SaveFileLoadCompletedSignal>(this);
            //_signalBus?.Unsubscribe<ServerLoginCompletedSignal>(this);
        }

       // private void InitServer() =>  _game.Init();//_liveOpsController.Init();
    }
}