using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Data;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.SaveLoad
{
    public class ProgressSaver : MonoBehaviour
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

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);

            _autoSaveCancellationTokenSource = new CancellationTokenSource();
            AutoSave(_autoSaveCancellationTokenSource.Token).Forget();
            
            _signalBus.Subscribe<SceneChangedSignal>(this, Save);
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

        private void OnDestroy()
        {
            _autoSaveCancellationTokenSource?.Cancel();
            _saveDelayCancellationTokenSource?.Cancel();
            _signalBus.Unsubscribe<SceneChangedSignal>(this);
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
    }
}
