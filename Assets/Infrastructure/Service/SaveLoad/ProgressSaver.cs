using System.Collections;
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
        
        private const bool IsAutoSaveEnabled = true;
        private const int AutoSaveInterval = 60;
        private const int AutoSaveDelay = 5;

        private bool _isReadyForSave = true;
        private Coroutine _autoSaveCoroutine;
        private Coroutine _saveDelayCoroutine;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            _signalBus.Subscribe<SceneChangedSignal>(this, Save);
            _autoSaveCoroutine = StartCoroutine(AutoSave());
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
            if (_autoSaveCoroutine != null)
            {
                StopCoroutine(_autoSaveCoroutine);
            }
            
            Save();
        }

        private void OnDestroy()
        {
            _signalBus.Unsubscribe<SceneChangedSignal>(this);
        }

        private IEnumerator AutoSave()
        {
            while (IsAutoSaveEnabled)
            {
                yield return new WaitForSeconds(AutoSaveInterval);
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
            
            if (_saveDelayCoroutine != null)
            {
                StopCoroutine(_saveDelayCoroutine);
            }
            
            _saveDelayCoroutine = StartCoroutine(SaveDelay());
        }

        private void Save()
        {
            _dataSaver.Save();
        }

        private IEnumerator SaveDelay()
        {
            yield return new WaitForSeconds(AutoSaveDelay);
            _isReadyForSave = true;
        }
    }
}
