using System.Collections.Generic;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private protected AudioSource m_AudioSource;
        
        private readonly Dictionary<string, AudioClip> _audioList = new Dictionary<string, AudioClip>();
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
        
        private protected AudioClip GetAudio(string audioClipName, string audioType)
        {
            if (_audioList.TryGetValue(audioClipName, out var audioClip))
            {
                return audioClip;
            }
            else
            {
                audioClip = _audioLoader.LoadAudio(audioClipName, audioType);
                return audioClip;
            }
        }
    }
}