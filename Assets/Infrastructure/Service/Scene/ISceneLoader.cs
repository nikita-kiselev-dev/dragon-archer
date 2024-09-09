using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Scene
{
    public interface ISceneLoader
    {
        public UniTask LoadAsync(string sceneName, Action onLoaded = null);
    }
}