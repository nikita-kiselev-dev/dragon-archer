using System;

namespace Infrastructure.Service.Asset
{
    public interface IOperationHandler<out T> : IOperationStatus, IDisposable
    {
        T Result { get; }
    }
}