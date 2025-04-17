using System;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.Service.Scene
{
    public class SceneService : ISceneService, IDisposable
    {
        private readonly ISignalBus _signalBus;
        private readonly ISceneLoader _sceneLoader;
        
        [Inject]
        public SceneService(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            _sceneLoader = new SceneLoader(_signalBus);
            Init();
        }
        
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _sceneLoader.PrepareSceneLoad(sceneName, onLoaded);
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<StartSceneChangeSignal>(this);
        }

        private void Init()
        {
            SceneManager.activeSceneChanged += OnSceneChanged; 
            _signalBus.Subscribe<StartSceneChangeSignal>(this, LoadScene);
        }

        private void LoadScene()
        {
            _sceneLoader.LoadAsync().Forget();
        }

        private void OnSceneChanged(
            UnityEngine.SceneManagement.Scene previousScene, 
            UnityEngine.SceneManagement.Scene currentScene)
        {
            if (currentScene.name != SceneConstants.BootstrapScene && currentScene.name != SceneConstants.StartScene)
            {
                _signalBus.Trigger<SceneChangedSignal>();
            }
        }
    }
}