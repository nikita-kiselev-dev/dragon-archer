using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;
using Object = UnityEngine.Object;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader, IDisposable
    {
        private readonly ISignalBus _signalBus;
        private readonly Dictionary<string, object> _cachedAssets = new();
        private readonly Dictionary<string, HashSet<GameObject>> _cachedGameObjects = new();

        [Inject]
        private MainAddressableAssetLoader(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<StartSceneChangeSignal>(this, ReleaseAllAssets);
        }
        
        public async UniTask<T> LoadAsync<T>(string key)
        {
            if (_cachedAssets.TryGetValue(key, out var cachedObject))
            {
                return (T)cachedObject;
            }
            
            var asyncOperationHandle = Addressables.LoadAssetAsync<T>(key);
            var task = asyncOperationHandle.ToUniTask();
            var loadedObject = await task;
            _cachedAssets.TryAdd(key, loadedObject);
            
            return loadedObject;
        }

        public async UniTask<T> InstantiateAsync<T>(string key, Transform parent = null)
        {
            await LoadAsync<GameObject>(key);
            
            var asyncOperationHandle = parent ? 
                Addressables.InstantiateAsync(key, parent) : 
                Addressables.InstantiateAsync(key);

            var task = asyncOperationHandle.ToUniTask();
            var loadedGameObject = await task;
            ConfigureCachedGameObject(key, loadedGameObject);
                
            return loadedGameObject.GetComponent<T>();
        }
        
        public void Release(string key, bool removeFromCache = true)
        {
            if (!_cachedAssets.TryGetValue(key, out var cachedAsset))
            {
                return;
            }

            var cachedGameObjects = new HashSet<GameObject>();
            
            if (cachedAsset is GameObject cachedGameObject &&
                _cachedGameObjects.TryGetValue(key, out cachedGameObjects))
            {
                cachedGameObjects.Remove(cachedGameObject);
                Addressables.ReleaseInstance(cachedGameObject);
            }

            if (cachedGameObjects != null && cachedGameObjects.Any())
            {
                return;
            }
            
            if (!removeFromCache)
            {
                return;
            }
            
            _cachedAssets.Remove(key);

            if (cachedAsset is not GameObject)
            {
                Addressables.Release(cachedAsset);
            }
        }

        void IDisposable.Dispose()
        {
            _signalBus.Unsubscribe<StartSceneChangeSignal>(this);
        }

        private void ConfigureCachedGameObject(string key, GameObject gameObject)
        {
            if (!_cachedGameObjects.ContainsKey(key))
            {
                _cachedGameObjects.Add(key, new HashSet<GameObject>());
            }
            
            _cachedGameObjects[key].Add(gameObject);
        }

        private void ReleaseAllAssets()
        {
            foreach (var cachedGameObjectType in _cachedGameObjects)
            {
                foreach (var gameObject in cachedGameObjectType.Value)
                {
                    gameObject.SetActive(false);
                    Addressables.ReleaseInstance(gameObject);
                }
            }
            
            _cachedGameObjects.Clear();
            
            foreach (var cachedAsset in _cachedAssets)
            {
                Release(cachedAsset.Key, false);

                if (cachedAsset.Value is not GameObject)
                {
                    Addressables.Release(cachedAsset.Value);
                }
            }

            _cachedAssets.Clear();
        }
    }
}