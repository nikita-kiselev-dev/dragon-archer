using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Data;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.SignalBus.MonoBehaviour;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class ProgressSaver : IProgressSaver, IDisposable
    {
        private const bool IsAutoSaveEnabled = true;
        private const int AutoSaveIntervalInSeconds = 15;
        private const int AutoSaveDelayInSeconds = 5;

        private readonly IDataSaver _dataSaver;
        private readonly IMainDataManager _mainDataManager;

        private bool _isReadyForSave = true;
        
        private CancellationTokenSource _autoSaveCancellationTokenSource;
        private CancellationTokenSource _saveDelayCancellationTokenSource;

        [Inject] 
        public ProgressSaver(ISignalBus signalBus, IDataSaver dataSaver, IMainDataManager mainDataManager)
        {
            _dataSaver = dataSaver;
            _mainDataManager = mainDataManager;
            
            signalBus.Subscribe<OnAwakeSignal>(this, OnAwake);
            signalBus.Subscribe<OnApplicationPauseSignal, bool>(this, OnApplicationPause);
            signalBus.Subscribe<OnApplicationQuitSignal>(this, OnApplicationQuit);
            signalBus.Subscribe<SceneChangedSignal>(this, Save);
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
        }
    }
}
