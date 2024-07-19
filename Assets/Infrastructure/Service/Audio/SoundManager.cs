namespace Infrastructure.Service.Audio
{
    public class SoundManager : AudioManager
    {
        private const string AudioType = "Sounds";
        
        public void Play(string audioClipName)
        {
            var audioClip = GetAudio(audioClipName, AudioType);
            m_AudioSource.PlayOneShot(audioClip);
        }
    }
}