using UnityEngine.Events;

namespace Core.View.ViewSignalManager
{
    public interface IViewSignalManager
    {
        public IViewSignalManager AddSignal(string signalName, UnityAction signal, string audioClipName = null);
        public IViewSignalManager AddSignal<T>(string signalName, UnityAction<T> signal, string audioClipName = null);
        public IViewSignalManager AddCloseSignal(UnityAction signal, string audioClipName = null);
        public IViewSignalManager AddRefreshSignal(UnityAction signal, string audioClipName = null);
        public UnityAction GetSignal(string signalName, bool withSound = true);
        public UnityAction<T> GetSignal<T>(string signalName, bool withSound = true);
        public UnityAction GetCloseSignal(bool withSound = true);
        public UnityAction GetRefreshSignal(bool withSound = true);
    }
}