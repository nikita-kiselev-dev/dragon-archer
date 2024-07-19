using System;
using System.Collections;
using Content.LoadingCurtain.Scripts;
using Content.LoadingCurtain.Scripts.Controller;
using UnityEngine;
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
        
        public IEnumerator LoadAsync(string sceneName, Action onLoaded = null)
        {
            var activeSceneName = SceneManager.GetActiveScene().name;
            
            if (activeSceneName == sceneName)
            {
                onLoaded?.Invoke();
                yield break;
            }

            if (sceneName != SceneInfo.StartScene)
            {
                _loadingCurtainController.Show();
                yield return new WaitForSeconds(LoadingCurtainInfo.ShowAnimationDuration);
            }
            
            var loadSceneAsync = SceneManager.LoadSceneAsync(sceneName);
            
            if (loadSceneAsync != null)
            {
                loadSceneAsync.completed += (_) =>
                {
                    onLoaded?.Invoke();
                    _loadingCurtainController.Hide();
                };
            }
        }

    }
}