using System;
using Content.LoadingCurtain.Scripts.Controller;
using Infrastructure.Service.Dto;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.SaveLoad;
using Infrastructure.Service.SaveLoad.Signals;
using Infrastructure.Service.SignalBus;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.Game
{
    public class GameBootstrapper : IStartable, IDisposable
    {
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDataManager _dataManager;
        [Inject] private readonly IDtoManager _dtoManager;
        [Inject] private readonly ILoadingCurtainController _loadingCurtainController;
        [Inject] private readonly ILiveOpsController _liveOpsController;
        [Inject] private readonly IGame _game;
        
        void IStartable.Start()
        {
            _signalBus.Subscribe<SaveFileLoadCompletedSignal>(this, InitServer);
            _signalBus.Subscribe<ServerLoginCompletedSignal, bool>(this, InitGame);
            _loadingCurtainController.Init();
            _dataManager.Init();
            _dtoManager.Init();
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<SaveFileLoadCompletedSignal>(this);
            _signalBus.Unsubscribe<ServerLoginCompletedSignal>(this);
        }

        private void InitServer()
        {
            _liveOpsController.Init();
        }

        private void InitGame(bool isLoginCompleted)
        {
            _loadingCurtainController.Hide();
            _game.Init();
        }
    }
}