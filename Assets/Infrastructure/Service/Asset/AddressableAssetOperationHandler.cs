using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.Service.Asset
{
    public class AddressableAssetOperationHandler<T> : IOperationHandler<T>
    {
        private AsyncOperationHandle<T> m_OperationHandle;

        public AddressableAssetOperationHandler(AsyncOperationHandle<T> operationHandle)
        {
            m_OperationHandle = operationHandle;
        }

        public bool IsSuccessful
            => m_OperationHandle.IsDone
               && m_OperationHandle.Status 
               == AsyncOperationStatus.Succeeded;

        public bool IsCompleted
            => m_OperationHandle.IsDone;

        private string Message { get; set; }

        public T Result
        {
            get
            {
                if (!IsSuccessful)
                {
                    return default;
                }

                var asset = m_OperationHandle.Result;
                if (asset != null)
                {
                    return asset;
                }

                Message
                    = $"'{m_OperationHandle.Result}' not assignable from '{typeof(T)}'"; 
                throw new MissingComponentException(Message);
            }
        }

        public void Release()
        {
            if (m_OperationHandle.IsValid())
            {
                Addressables.Release(m_OperationHandle);    
            }
        }
        
        public void Dispose() => Release();
    }
}