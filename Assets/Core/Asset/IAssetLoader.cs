using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Asset
{
    public interface IAssetLoader
    {
        public UniTask<T> LoadAsync<T>(string key);
        public UniTask<T> InstantiateAsync<T>(string key, Transform parent = null);
        public void Release(string key, bool removeFromCache = true);
    }
}