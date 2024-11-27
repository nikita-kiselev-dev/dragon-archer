using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader
    {
        private readonly Dictionary<string, object> _loadedAssets = new();
        private readonly Dictionary<string, GameObject> _loadedGameObjects = new();

        private MainAddressableAssetLoader(ISignalBus signalBus)
        {
            signalBus.Subscribe<SceneChangedSignal>(this, Dispose);
        }
        
        public async UniTask<T> LoadAsync<T>(string key)
        {
            if (_loadedAssets.TryGetValue(key, out var cachedObject))
            {
                return (T)cachedObject;
            }
            
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var task = asyncOperationHandle.ToUniTask();
            var loadedObject = await task;
            
            _loadedAssets.TryAdd(key, loadedObject);
            
            return loadedObject;
        }

        public async UniTask<T> InstantiateAsync<T>(string key, Transform parent = null)
        {
            if (_loadedGameObjects.TryGetValue(key, out var cachedGameObject))
            {
                return cachedGameObject.GetComponent<T>();
            }
            
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);

            var task = asyncOperationHandle.ToUniTask();
            var loadedGameObject = await task;
            var loadedObject = loadedGameObject.GetComponent<T>();
            
            _loadedGameObjects.TryAdd(key, loadedGameObject);
            
            return loadedObject;
        }
        
        public void Release(string key)
        {
            if (_loadedAssets.TryGetValue(key, out var loadedAsset))
            {
                Addressables.Release(loadedAsset);
                _loadedAssets.Remove(key);
            }
            
            if (_loadedGameObjects.TryGetValue(key, out var loadedGameObject))
            {
                Addressables.ReleaseInstance(loadedGameObject);
                Object.Destroy(loadedGameObject);
                _loadedGameObjects.Remove(key);
            }
        }

        private void Dispose()
        {
            foreach (var loadedAsset in _loadedAssets
                         .Where(loadedAsset => loadedAsset.Value is not null))
            {
                Addressables.Release(loadedAsset.Value);
            }
            
            _loadedAssets.Clear();
            
            foreach (var loadedGameObject in _loadedGameObjects
                         .Where(loadedGameObject => loadedGameObject.Value is not null))
            {
                Object.Destroy(loadedGameObject.Value);
            }
            
            _loadedGameObjects.Clear();
        }
    }
}