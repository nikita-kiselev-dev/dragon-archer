using System;
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
        [Inject] private readonly IGame _game;
        
        void IStartable.Start()
        {
            _signalBus.Subscribe<SaveFileLoadCompletedSignal>(this, InitGame);
            _dataManager.Init();
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<SaveFileLoadCompletedSignal>(this);
        }

        private void InitGame()
        {
            _game.Init();
        }
    }
}