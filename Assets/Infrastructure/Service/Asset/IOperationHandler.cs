using System;
using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Asset
{
    public interface IOperationHandler<T> : IDisposable
    {
        public UniTask<T> Result();
    }
}