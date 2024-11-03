using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Scene
{
    public interface ISceneLoader
    {
        public UniTaskVoid LoadAsync(string sceneName, Action onLoaded = null);
    }
}