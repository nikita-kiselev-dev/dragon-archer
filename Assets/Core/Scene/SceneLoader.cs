using System;
using Core.Scene.Signals;
using Core.SignalBus;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Core.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ISignalBus _signalBus;

        private string _sceneNameToOpen;
        private Action _onSceneLoadedCallback;
        
        public SceneLoader(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public void PrepareSceneLoad(string sceneName, Action onSceneLoadedCallback = null)
        {
            _sceneNameToOpen = sceneName;
            _onSceneLoadedCallback = onSceneLoadedCallback;
            var activeSceneName = SceneManager.GetActiveScene().name;
            
            if (activeSceneName == _sceneNameToOpen)
            {
                onSceneLoadedCallback?.Invoke();
                return;
            }

            var isStartScene = _sceneNameToOpen == SceneConstants.StartScene;

            if (!isStartScene)
            {
                _signalBus.Trigger<OnChangeSceneRequestSignal>();
            }
            else
            {
                LoadAsync().Forget();
            }
        }

        public async UniTaskVoid LoadAsync()
        {
            var isStartScene = _sceneNameToOpen == SceneConstants.StartScene;
            var secondsToWait = isStartScene ? 0 : SceneConstants.SceneLoadDelay;
            await UniTask.WaitForSeconds(secondsToWait);
            var loadSceneAsync = SceneManager.LoadSceneAsync(_sceneNameToOpen).ToUniTask();
            await loadSceneAsync;
            _onSceneLoadedCallback?.Invoke();
        }
    }
}