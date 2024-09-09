using System;
using Content.LoadingCurtain.Scripts.Controller;
using Cysharp.Threading.Tasks;
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
        public SceneService(ISignalBus signalBus, ILoadingCurtainController loadingCurtainController)
        {
            _signalBus = signalBus;
            _sceneLoader = new SceneLoader(loadingCurtainController);
            SceneManager.activeSceneChanged += OnSceneChanged; 
        }
        
        public void LoadScene(string sceneName, Action onLoaded = null)
        {
            _sceneLoader.LoadAsync(sceneName, onLoaded).Forget();
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