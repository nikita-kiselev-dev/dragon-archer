using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public class AudioLoader : IAudioLoader
    {
        private const string AudioFileFormat = ".wav";
        private readonly IAssetLoader _assetLoader;
        
        public AudioLoader(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }
        
        public async UniTask<AudioClip> LoadAudio(string audioClipName, string audioType)
        {
            var audioClipAddressPath = audioType + "/" + audioClipName + AudioFileFormat;
            var audioClip = await _assetLoader.LoadAsync<AudioClip>(audioClipAddressPath);

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