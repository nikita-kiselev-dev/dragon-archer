using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public interface IAudioLoader
    {
        public UniTask<AudioClip> LoadAudio(string audioClipName, string audioType);
    }
}