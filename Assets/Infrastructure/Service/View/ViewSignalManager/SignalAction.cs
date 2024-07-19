using System;

namespace Infrastructure.Service.View.ViewSignalManager
{
    public class SignalAction
    {
        public readonly Delegate SignalDelegate;
        public readonly string AudioClipName;
        public bool HaveAudio => !string.IsNullOrEmpty(AudioClipName);

        public SignalAction(Delegate signalDelegate, string audioClipName)
        {
            SignalDelegate = signalDelegate;
            AudioClipName = audioClipName;
        }
    }
}