using UnityEngine.Events;

namespace Infrastructure.Service.SignalBus
{
    public interface ISignalBus
    {
        public void Subscribe<T>(object listener, UnityAction action);
        public void Subscribe<T, U>(object listener, UnityAction<U> action);
        public void Unsubscribe<T>(object listener);
        public void Trigger<T>();
        public void Trigger<T, U>(U arg);
    }
}