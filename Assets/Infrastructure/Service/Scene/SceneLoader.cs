using System;
using Content.LoadingCurtain.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine.SceneManagement;

namespace Infrastructure.Service.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ISignalBus _signalBus;
        
        public SceneLoader(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public async UniTaskVoid LoadAsync(string sceneName, Action onLoaded = null)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;
            
            if (activeSceneName == sceneName)
            {
                onLoaded?.Invoke();
                return;
            }

            var isStartScene = sceneName == SceneInfo.StartScene;

            if (!isStartScene)
            {
                _signalBus.Trigger<OnChangeSceneRequestSignal>();
                await UniTask.WaitForSeconds(LoadingCurtainInfo.ShowAnimationDuration);
            }
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            await loadSceneAsync;
            onLoaded?.Invoke();
        }
    }
}