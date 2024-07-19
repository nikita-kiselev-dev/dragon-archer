using System;
using System.Collections;

namespace Infrastructure.Service.Scene
{
    public interface ISceneLoader
    {
        public IEnumerator LoadAsync(string sceneName, Action onLoaded = null);
    }
}