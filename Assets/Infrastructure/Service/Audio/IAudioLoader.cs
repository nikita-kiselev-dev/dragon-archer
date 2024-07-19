using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public interface IAudioLoader
    {
        public AudioClip LoadAudio(string audioClipName, string audioType);
    }
}