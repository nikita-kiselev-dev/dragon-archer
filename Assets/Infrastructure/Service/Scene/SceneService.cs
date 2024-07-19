using System;
using Content.LoadingCurtain.Scripts.Controller;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.Service.Scene
{
    public class SceneService : ISceneService, IDisposable
    {
        [Inject] private readonly ICoroutineRunner _coroutineRunner;
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly ILoadingCurtainController _loadingCurtainController;
        
        private ISceneLoader _sceneLoader;
        
        [Inject]
        public void Init()
        {
            _sceneLoader = new SceneLoader(_loadingCurtainController);
            SceneManager.activeSceneChanged += OnSceneChanged; 
        }
        
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _coroutineRunner.StartCoroutine(_sceneLoader.LoadAsync(sceneName, onLoaded));
        }

        void IDisposable.Dispose()
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }

        private void OnSceneChanged(
            UnityEngine.SceneManagement.Scene previousScene, 
            UnityEngine.SceneManagement.Scene currentScene)
        {
            if (currentScene.name != SceneInfo.BootstrapScene && currentScene.name != SceneInfo.StartScene)
            {
                _signalBus.Trigger<SceneChangedSignal>();
            }
        }
    }
}