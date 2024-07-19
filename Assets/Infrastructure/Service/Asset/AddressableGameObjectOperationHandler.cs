using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Infrastructure.Service.Asset
{
    public class AddressableGameObjectOperationHandler<T> : IOperationHandler<T>
    {
        private readonly AsyncOperationHandle<GameObject> m_OperationHandle;
        private T m_Result;
        
        public AddressableGameObjectOperationHandler(AsyncOperationHandle<GameObject> operationHandle)
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
                if (m_Result != null)
                {
                    return m_Result;
                }
                
                if (!IsSuccessful)
                {
                    return default;
                }

                m_Result = m_OperationHandle.Result.GetComponent<T>();
                if (m_Result != null)
                {
                    return m_Result;
                }

                Message
                    = $"'{m_OperationHandle.Result}' not assignable from '{typeof(T)}'"; 
                throw new MissingComponentException(Message);
            }
        }

        private void ReleaseInstance()
        {
            if (m_OperationHandle.IsValid())
            {
                Addressables.Release(m_OperationHandle);
            }
        }

        public void Dispose()
        {
            ReleaseInstance();
        }
    }
}