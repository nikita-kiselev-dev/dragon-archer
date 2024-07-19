using System;

namespace Infrastructure.Service.SignalBus
{
    public interface ISignalBus
    {
        public void Subscribe<ISignal>(object listener, Action action);
        public void Unsubscribe<ISignal>(object listener);
        public void Trigger<ISignal>();
    }
}