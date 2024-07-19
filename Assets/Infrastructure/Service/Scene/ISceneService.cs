using System;

namespace Infrastructure.Service.Scene
{
    public interface ISceneService
    {
        public void LoadScene(string sceneName, Action onLoaded = null);
    }
}