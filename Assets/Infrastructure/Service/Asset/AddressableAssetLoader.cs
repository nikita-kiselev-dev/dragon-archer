using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Service.Asset
{
    public class AddressableAssetLoader : IAssetLoader
    {
        public IOperationHandler<T> LoadAsset<T>(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var op = new AddressableAssetOperationHandler<T>(asyncOperationHandle);
            asyncOperationHandle.WaitForCompletion();
            return op;
        }

        public IOperationHandler<T> Instantiate<T>(object key, Transform parent = null)
        {
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);

            var op = new AddressableGameObjectOperationHandler<T>(asyncOperationHandle);
            asyncOperationHandle.WaitForCompletion();
            return op;
        }
    }
}