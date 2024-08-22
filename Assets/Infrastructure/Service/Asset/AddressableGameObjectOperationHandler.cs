using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Service.Asset
{
    public class AddressableGameObjectOperationHandler<T> : IOperationHandler<T>
    {
        private readonly UniTask<GameObject> _operationHandle;
        
        public AddressableGameObjectOperationHandler(UniTask<GameObject> operationHandle)
        {
            _operationHandle = operationHandle;
        }

        public async UniTask<T> Result()
        {
            try
            {
                var asset = await _operationHandle;
                return asset.GetComponent<T>();
            }
            catch (Exception exception)
            {
                throw;
            }

            return default;
        }
        
        
        public void Dispose() => Release();
        
        private void Release()
        {
            Addressables.Release(_operationHandle);    
        }
    }
}