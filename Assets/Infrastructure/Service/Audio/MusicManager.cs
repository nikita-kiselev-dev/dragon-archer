using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public class MusicManager : AudioManager
    {
        private const string AudioType = "Music";
        
        private const float FadeInAudioVolume = 0.7f;
        private const float FadeOutAudioVolume = 1.0f;

        private const float FadeInDuration = 1.5f;
        private const float FadeOutDuration = 1.5f;
        
        public void Play(string audioClipName, bool isLooped = true)
        {
            var isNewAudioClipAlreadyPlaying = m_AudioSource.isPlaying && audioClipName == m_AudioSource.clip.name;
            
            if (isNewAudioClipAlreadyPlaying)
            {
                return;
            }

            var audioClip = GetAudio(audioClipName, AudioType);
            
            if (m_AudioSource.isPlaying)
            {
                Switch(audioClip, isLooped);
            }
            else
            {
                PlayMusic(audioClip, isLooped);
            }
        }

        private void PlayMusic(AudioClip audioClip, bool isLooped = true)
        {
            m_AudioSource.clip = audioClip;
            m_AudioSource.loop = isLooped;
            m_AudioSource.Play();
        }
        
        private void Switch(AudioClip audioClip, bool isLooped = true)
        {
            var sequence = DOTween.Sequence();
            sequence
                .Append(m_AudioSource.DOFade(FadeInAudioVolume, FadeInDuration))
                .AppendCallback(() => PlayMusic(audioClip, isLooped))
                .Append(m_AudioSource.DOFade(FadeOutAudioVolume, FadeOutDuration))
                .Play();
        }
    }
}