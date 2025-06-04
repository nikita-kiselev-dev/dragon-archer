using System;

namespace Core.Scene
{
    public interface ISceneService
    {
        public void LoadScene(string sceneName, Action onLoaded = null);
    }
}