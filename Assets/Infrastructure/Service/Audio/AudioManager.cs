using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private protected AudioSource m_AudioSource;
        
        private readonly Dictionary<string, AudioClip> _audioList = new();
        private IAudioLoader _audioLoader;

        public void Init(IAudioLoader audioLoader)
        {
            _audioLoader = audioLoader;
        }
        
        public void Pause()
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Pause();
            }
        }

        public void Stop()
        {
            if (m_AudioSource.isPlaying)
            {
                m_AudioSource.Stop();
            }
        }

        public void SetVolume(float volume)
        {
            m_AudioSource.volume = volume;
        }
        
        private protected async UniTask<AudioClip> GetAudio(string audioClipName, string audioType)
        {
            if (_audioList.TryGetValue(audioClipName, out var audioClip))
            {
                return audioClip;
            }
            else
            {
                audioClip = await _audioLoader.LoadAudio(audioClipName, audioType);
                _audioList.Add(audioClipName, audioClip);
                return audioClip;
            }
        }
    }
}