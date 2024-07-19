using System;
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
        
        public AudioClip LoadAudio(string audioClipName, string audioType)
        {
            var audioClipAddressPath = audioType + "/" + audioClipName + AudioFileFormat;
            var audioClip = _assetLoader.LoadAsset<AudioClip>(audioClipAddressPath).Result;

            if (audioClip)
            {
                return audioClip;
            }
            else
            {
                throw new ArgumentNullException($"AudioController: can't load audio clip with name {audioClipName}!");
            }
        }
    }
}