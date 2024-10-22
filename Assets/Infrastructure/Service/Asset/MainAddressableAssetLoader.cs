using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader
    {
        public async UniTask<T> LoadAssetAsync<T>(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var task = asyncOperationHandle.ToUniTask();
            var asset = await task;
            
            return asset;
        }

        public async UniTask<T> InstantiateAsync<T>(object key, Transform parent = null)
        {
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);

            var task = asyncOperationHandle.ToUniTask();
            var asset = await task;
            var gameObject = asset.GetComponent<T>();
            
            return gameObject;
        }
    }
}