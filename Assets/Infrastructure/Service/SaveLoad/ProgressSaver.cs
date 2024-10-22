using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Data;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.SignalBus.Monobehavior;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class ProgressSaver : ControlEntity, IProgressSaver, IDisposable
    {
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDataSaver _dataSaver;
        [Inject] private readonly IMainDataManager _mainDataManager;
        
        private const bool IsAutoSaveEnabled = true;
        private const int AutoSaveIntervalInSeconds = 15;
        private const int AutoSaveDelayInSeconds = 5;

        private bool _isReadyForSave = true;
        
        private CancellationTokenSource _autoSaveCancellationTokenSource;
        private CancellationTokenSource _saveDelayCancellationTokenSource;

        protected override async UniTask Init()
        {
            _signalBus.Subscribe<OnAwakeSignal>(this, OnAwake);
            _signalBus.Subscribe<OnApplicationPauseSignal, bool>(this, OnApplicationPause);
            _signalBus.Subscribe<OnApplicationQuitSignal>(this, OnApplicationQuit);
            _signalBus.Subscribe<SceneChangedSignal>(this, Save);
            await UniTask.CompletedTask;
        }

        private void OnAwake()
        {
            _autoSaveCancellationTokenSource = new CancellationTokenSource();
            AutoSave(_autoSaveCancellationTokenSource.Token).Forget();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus)
            {
                TrySave();
            }
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        private async UniTask AutoSave(CancellationToken cancellationTokenSource)
        {
            while (!cancellationTokenSource.IsCancellationRequested)
            {
                await UniTask.WaitForSeconds(
                    AutoSaveIntervalInSeconds, 
                    cancellationToken: cancellationTokenSource);
                
                TrySave();
            }
        }

        private void TrySave()
        {
            if (!_isReadyForSave)
            {
                return;
            }
            
            Save();
            _isReadyForSave = false;
            
            _saveDelayCancellationTokenSource?.Cancel();
            _saveDelayCancellationTokenSource = new CancellationTokenSource();
            SaveDelay(_saveDelayCancellationTokenSource.Token).Forget();
        }
        
        private async UniTask SaveDelay(CancellationToken cancellationTokenSource)
        {
            await UniTask.WaitForSeconds(
                AutoSaveDelayInSeconds, 
                cancellationToken: cancellationTokenSource);
            
            _isReadyForSave = true;
        }

        private void Save()
        {
            _mainDataManager.SetLocalTime(DateTime.Now.ToUniversalTime());
            _dataSaver.Save();
        }
        
        void IDisposable.Dispose()
        {
            _autoSaveCancellationTokenSource?.Cancel();
            _saveDelayCancellationTokenSource?.Cancel();
            _signalBus?.Unsubscribe<OnAwakeSignal>(this);
            _signalBus?.Unsubscribe<OnApplicationPauseSignal>(this);
            _signalBus?.Unsubscribe<OnApplicationQuitSignal>(this);
            _signalBus?.Unsubscribe<SceneChangedSignal>(this);
        }
    }
}
