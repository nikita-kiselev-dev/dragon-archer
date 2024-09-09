using Content.LoadingCurtain.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader
    {
        private readonly ISignalBus _signalBus;
        
        [Inject]
        public MainAddressableAssetLoader(ISignalBus signalBus)
        {
            _signalBus = signalBus;
        }
        
        public async UniTask<T> LoadAssetAsync<T>(object key)
        {
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var task = asyncOperationHandle.ToUniTask().ToAsyncLazy();
            _signalBus.Trigger<AddLoadingOperationSignal, UniTask>(task.Task);
            var asset = await task;
            
            return asset;
        }

        public async UniTask<T> InstantiateAsync<T>(object key, Transform parent = null)
        {
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);

            var task = asyncOperationHandle.ToUniTask().ToAsyncLazy();
            _signalBus.Trigger<AddLoadingOperationSignal, UniTask>(task.Task);
            var asset = await task;
            var gameObject = asset.GetComponent<T>();
            
            return gameObject;
        }
    }
}