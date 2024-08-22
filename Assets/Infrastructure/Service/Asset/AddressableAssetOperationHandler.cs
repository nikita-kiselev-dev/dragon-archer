using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Infrastructure.Service.Asset
{
    public class AddressableAssetOperationHandler<T> : IOperationHandler<T>
    {
        private readonly UniTask<T> _operationHandle;
        
        public AddressableAssetOperationHandler(UniTask<T> operationHandle)
        {
            _operationHandle = operationHandle;
        }

        public async UniTask<T> Result()
        {
            try
            {
                var asset = await _operationHandle;
                return asset;
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