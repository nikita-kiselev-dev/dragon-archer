using System;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine.SceneManagement;
using VContainer;

namespace Infrastructure.Service.Scene
{
    public class SceneService : ISceneService
    {
        private readonly ISignalBus _signalBus;
        private readonly ISceneLoader _sceneLoader;
        
        [Inject]
        public SceneService(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            _sceneLoader = new SceneLoader(_signalBus);
            SceneManager.activeSceneChanged += OnSceneChanged; 
        }
        
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _sceneLoader.LoadAsync(sceneName, onLoaded).Forget();
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