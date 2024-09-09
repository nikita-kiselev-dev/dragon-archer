using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace Infrastructure.Service.Audio
{
    public class MusicManager : AudioManager
    {
        private const string AudioType = "Music";

        private const float FadeInDuration = 1.0f;
        private const float FadeOutDuration = 1.0f;
        
        public async UniTaskVoid Play(string audioClipName, bool isLooped = true)
        {
            var isNewAudioClipAlreadyPlaying = m_AudioSource.isPlaying && audioClipName == m_AudioSource.clip.name;
            
            if (isNewAudioClipAlreadyPlaying)
            {
                return;
            }

            var audioClip = await GetAudio(audioClipName, AudioType);
            
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
                .Append(m_AudioSource.DOFade(0, FadeInDuration))
                .AppendCallback(() => PlayMusic(audioClip, isLooped))
                .Append(m_AudioSource.DOFade(1, FadeOutDuration))
                .Play();
        }
    }
}