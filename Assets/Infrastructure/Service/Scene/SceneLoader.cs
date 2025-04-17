using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine.SceneManagement;

namespace Infrastructure.Service.Scene
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

            var isStartScene = _sceneNameToOpen == SceneInfo.StartScene;

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
            var isStartScene = _sceneNameToOpen == SceneInfo.StartScene;
            var secondsToWait = isStartScene ? 0 : 1.0f;
            await UniTask.WaitForSeconds(secondsToWait);
            var loadSceneAsync = SceneManager.LoadSceneAsync(_sceneNameToOpen).ToUniTask();
            await loadSceneAsync;
            _onSceneLoadedCallback?.Invoke();
        }
    }
}