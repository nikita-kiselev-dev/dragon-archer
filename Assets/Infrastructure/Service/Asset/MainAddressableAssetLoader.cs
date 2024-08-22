using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader
    {
        public void Instantiate(object key, Transform parent = null)
        {
            if (parent)
            {
                Addressables.InstantiateAsync(key, parent);
            }
            else
            {
                Addressables.InstantiateAsync(key);
            }
        }

        public async UniTask<T> LoadAssetAsync<T>(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var task = asyncOperationHandle.ToUniTask();
            var operationHandler = new AddressableAssetOperationHandler<T>(task);
            var asset = await operationHandler.Result();
            
            return asset;
        }

        public async UniTask<T> InstantiateAsync<T>(object key, Transform parent = null)
        {
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);
            
            var task = asyncOperationHandle.ToUniTask(); 
            var operationHandler = new AddressableGameObjectOperationHandler<T>(task);
            var gameObject = await operationHandler.Result();
            
            return gameObject;
        }
    }
}