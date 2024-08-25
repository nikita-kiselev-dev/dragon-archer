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
            var asset = await _operationHandle;
            return asset;
        }
        
        
        public void Dispose() => Release();
        
        private void Release()
        {
            Addressables.Release(_operationHandle);    
        }
    }
}