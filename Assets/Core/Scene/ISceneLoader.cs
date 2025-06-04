using System;
using Cysharp.Threading.Tasks;

namespace Core.Scene
{
    public interface ISceneLoader
    {
        void PrepareSceneLoad(string sceneName, Action onSceneLoadedCallback = null);
        UniTaskVoid LoadAsync();
    }
}