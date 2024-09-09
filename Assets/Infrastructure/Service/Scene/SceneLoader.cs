using System;
using Content.LoadingCurtain.Scripts;
using Content.LoadingCurtain.Scripts.Controller;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Infrastructure.Service.Scene
{
    public class SceneLoader : ISceneLoader
    {
        private readonly ILoadingCurtainController _loadingCurtainController;
        
        public SceneLoader(ILoadingCurtainController loadingCurtainController)
        {
            _loadingCurtainController = loadingCurtainController;
        }
        
        public async UniTask LoadAsync(string sceneName, Action onLoaded = null)
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
                _loadingCurtainController.Show();
                await UniTask.WaitForSeconds(LoadingCurtainInfo.ShowAnimationDuration);
            }
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName).ToUniTask();
            await loadSceneAsync;
            onLoaded?.Invoke();
        }
    }
}