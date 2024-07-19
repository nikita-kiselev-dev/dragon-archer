using UnityEngine;

namespace Infrastructure.Service.Asset
{
    public interface IAssetLoader
    {
        IOperationHandler<T> LoadAsset<T>(object key); 
        IOperationHandler<T> Instantiate<T>(object key, Transform parent = null);
    }
}