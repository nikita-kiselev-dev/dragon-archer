using Infrastructure.Service.Asset;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.Audio
{
    public class AudioService : MonoBehaviour, IAudioService
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        
        [SerializeField] private SoundManager m_SoundSource;
        [SerializeField] private MusicManager m_MusicSource;
        
        public static IAudioService Instance;

        [Inject]
        private void Init()
        {
            Instance = this;
            DontDestroyOnLoad(this);

            var audioLoader = new AudioLoader(_assetLoader);
            m_SoundSource.Init(audioLoader);
            m_MusicSource.Init(audioLoader);
        }
        
        public void PlaySound(string audioClipName)
        {
            m_SoundSource.Play(audioClipName);
        }

        public void SetSoundsVolume(float volume)
        {
            m_SoundSource.SetVolume(volume);
        }

        public void PlayMusic(string audioClipName)
        {
            m_MusicSource.Play(audioClipName);
        }

        public void SetMusicVolume(float volume)
        {
            m_MusicSource.SetVolume(volume);
        }
    }
}