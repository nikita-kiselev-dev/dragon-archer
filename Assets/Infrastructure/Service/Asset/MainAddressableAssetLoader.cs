using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Scene.Signals;
using Infrastructure.Service.SignalBus;
using UnityEngine;
using UnityEngine.AddressableAssets;
using VContainer;

namespace Infrastructure.Service.Asset
{
    public class MainAddressableAssetLoader : IAssetLoader, IDisposable
    {
        private readonly ISignalBus _signalBus;
        private readonly Dictionary<string, object> _cachedAssets = new();
        private readonly Dictionary<string, HashSet<GameObject>> _cachedInstances = new();

        [Inject]
        private MainAddressableAssetLoader(ISignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<StartSceneChangeSignal>(this, ReleaseAllAssets);
        }

        public async UniTask<T> LoadAsync<T>(string key)
        {
            if (_cachedAssets.TryGetValue(key, out var cached)) return (T)cached;
            var handle = Addressables.LoadAssetAsync<T>(key);
            var result = await handle.ToUniTask();
            _cachedAssets[key] = result;
            return result;
        }

        public async UniTask<T> InstantiateAsync<T>(string key, Transform parent = null)
        {
            await LoadAsync<GameObject>(key);
            var handle = Addressables.InstantiateAsync(key, parent);
            var instance = await handle.ToUniTask();
            CacheInstance(key, instance);
            return instance.GetComponent<T>();
        }

        public void Release(string key, bool removeFromCache = true)
        {
            if (!_cachedAssets.TryGetValue(key, out var asset)) return;
            var assetIsGameObject = asset is GameObject;
            var instances = new HashSet<GameObject>();

            if (assetIsGameObject && _cachedInstances.TryGetValue(key, out instances))
            {
                var instance = (GameObject)asset;
                Addressables.ReleaseInstance(instance);
                instances.Remove(instance);
            }

            if (instances != null && instances.Any()) return;
            if (!removeFromCache) return;
            _cachedAssets.Remove(key);
            Addressables.Release(asset);
        }

        public void Dispose()
        {
            _signalBus.Unsubscribe<StartSceneChangeSignal>(this);
        }

        private void CacheInstance(string key, GameObject instance)
        {
            if (!_cachedInstances.TryGetValue(key, out var list))
            {
                list = new HashSet<GameObject>();
                _cachedInstances[key] = list;
            }

            list.Add(instance);
        }

        private void ReleaseAllAssets()
        {
            foreach (var instanceSet in _cachedInstances.Values.SelectMany(set => set))
            {
                instanceSet.SetActive(false);
                Addressables.ReleaseInstance(instanceSet);
            }

            _cachedInstances.Clear();

            foreach (var cachedAsset in _cachedAssets.Values)
            {
                Addressables.Release(cachedAsset);
            }

            _cachedAssets.Clear();
        }
    }
}
