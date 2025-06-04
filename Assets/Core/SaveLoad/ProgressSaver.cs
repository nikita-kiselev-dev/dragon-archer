using System;
using System.Threading;
using Core.MonoBehaviour.Scripts;
using Core.Scene.Signals;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using VContainer;
using VContainer.Unity;

namespace Core.SaveLoad
{
    public class ProgressSaver : IProgressSaver, IStartable, IDisposable
    {
        private const bool IsAutoSaveEnabled = true;
        private const int AutoSaveIntervalInSeconds = 15;
        private const int AutoSaveDelayInSeconds = 5;

        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDataSaver _dataSaver;

        private bool _isReadyForSave = true;
        
        private CancellationTokenSource _autoSaveCancellationTokenSource;
        private CancellationTokenSource _saveDelayCancellationTokenSource;
        
        void IStartable.Start()
        {
            _signalBus.Subscribe<OnAwakeSignal>(this, OnAwake);
            _signalBus.Subscribe<OnApplicationPauseSignal, bool>(this, OnApplicationPause);
            _signalBus.Subscribe<OnApplicationQuitSignal>(this, OnApplicationQuit);
            _signalBus.Subscribe<SceneChangedSignal>(this, Save);
        }
        
        void IDisposable.Dispose()
        {
            _autoSaveCancellationTokenSource?.Cancel();
            _saveDelayCancellationTokenSource?.Cancel();

            if (_signalBus is null)
            {
                return;
            }
            
            _signalBus.Unsubscribe<OnAwakeSignal>(this);
            _signalBus.Unsubscribe<OnApplicationPauseSignal>(this);
            _signalBus.Unsubscribe<OnApplicationQuitSignal>(this);
            _signalBus.Unsubscribe<SceneChangedSignal>(this);
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
            _dataSaver.SaveData();
        }
    }
}
