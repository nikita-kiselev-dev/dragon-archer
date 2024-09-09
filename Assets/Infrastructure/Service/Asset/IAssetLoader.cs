using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Service.Asset
{
    public interface IAssetLoader
    {
        public UniTask<T> LoadAssetAsync<T>(object key); 
        public UniTask<T> InstantiateAsync<T>(object key, Transform parent = null);
    }
}