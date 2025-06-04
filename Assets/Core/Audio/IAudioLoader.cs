using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Audio
{
    public interface IAudioLoader
    {
        public UniTask<AudioClip> LoadAudio(string audioClipName, string audioType);
    }
}