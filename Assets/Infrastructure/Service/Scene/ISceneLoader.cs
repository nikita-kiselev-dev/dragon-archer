using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Scene
{
    public interface ISceneLoader
    {
        void PrepareSceneLoad(string sceneName, Action onSceneLoadedCallback = null);
        UniTaskVoid LoadAsync();
    }
}