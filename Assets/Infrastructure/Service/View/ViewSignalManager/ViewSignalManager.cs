using System.Collections.Generic;
using Infrastructure.Service.Audio;
using Infrastructure.Service.View.ViewManager;
using UnityEngine.Events;

namespace Infrastructure.Service.View.ViewSignalManager
{
    public class ViewSignalManager : IViewSignalManager
    {
        private readonly Dictionary<string, SignalAction> _signals = new();
        
        public IViewSignalManager AddSignal(string signalName, UnityAction signal, string audioClipName = null)
        {
            var signalAction = new SignalAction(signal, audioClipName);
            _signals.Add(signalName, signalAction);
            return this;
        }
        
        public IViewSignalManager AddSignal<T>(string signalName, UnityAction<T> signal, string audioClipName = null)
        {
            var signalAction = new SignalAction(signal, audioClipName);
            _signals.Add(signalName, signalAction);
            return this;
        }

        public IViewSignalManager AddCloseSignal(UnityAction signal, string audioClipName = null)
        {
            var signalAction = new SignalAction(signal, audioClipName);
            _signals.Add(ViewInfo.CloseSignal, signalAction);
            return this;
        }

        public IViewSignalManager AddRefreshSignal(UnityAction signal, string audioClipName = null)
        {
            var signalAction = new SignalAction(signal, audioClipName);
            _signals.Add(ViewInfo.RefreshSignal, signalAction);
            return this;
        }

        public UnityAction GetSignal(string signalName, bool withSound = true)
        {
            var signal = _signals[signalName];
            return PlayWithSound(signal, withSound);
        }

        public UnityAction<T> GetSignal<T>(string signalName, bool withSound = true)
        {
            var signal = _signals[signalName];
            
            switch (withSound)
            {
                case true when signal.HaveAudio:
                    return arg =>
                    {
                        (signal.SignalDelegate as UnityAction<T>)?.Invoke(arg); 
                        PlayCustomSound(signal.AudioClipName);
                    };
                case true:
                    return arg =>
                    {
                        (signal.SignalDelegate as UnityAction<T>)?.Invoke(arg); 
                        PlayDefaultSound();
                    };
                default:
                    return arg =>
                    {
                        (signal.SignalDelegate as UnityAction<T>)?.Invoke(arg);
                    };
            }
        }
        
        public UnityAction GetCloseSignal(bool withSound = true)
        {
            var signal = _signals[ViewInfo.CloseSignal];
            return PlayWithSound(signal, withSound);
        }

        public UnityAction GetRefreshSignal(bool withSound = true)
        {
            var signal = _signals[ViewInfo.RefreshSignal];
            return PlayWithSound(signal, withSound);
        }
        
        private UnityAction PlayWithSound(SignalAction signalAction, bool withSound)
        {
            switch (withSound)
            {
                case true when signalAction.HaveAudio:
                    return () =>
                    {
                        (signalAction.SignalDelegate as UnityAction)?.Invoke();
                        PlayCustomSound(signalAction.AudioClipName);
                    };
                case true:
                    return () =>
                    {
                        (signalAction.SignalDelegate as UnityAction)?.Invoke();
                        PlayDefaultSound();
                    };
                default:
                    return signalAction.SignalDelegate as UnityAction;
            }
        }

        private void PlayDefaultSound() => AudioController.Instance.PlaySound(SoundList.ClickSound0);
        private void PlayCustomSound(string audioClipName) => AudioController.Instance.PlaySound(audioClipName);
    }
}