using System;
using Core.Asset;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.Audio
{
    public class AudioLoader : IAudioLoader
    {
        private readonly IAssetLoader _assetLoader;
        
        public AudioLoader(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }
        
        public async UniTask<AudioClip> LoadAudio(string audioClipName, string audioType)
        {
            var audioClip = await _assetLoader.LoadAsync<AudioClip>(audioClipName);

            if (audioClip)
            {
                return audioClip;
            }
            else
            {
                throw new ArgumentNullException($"{GetType().Name}: can't load audio clip with name {audioClipName}!");
            }
        }
    }
}