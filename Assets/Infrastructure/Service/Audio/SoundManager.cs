using Cysharp.Threading.Tasks;

namespace Infrastructure.Service.Audio
{
    public class SoundManager : AudioManager
    {
        private const string AudioType = "Sounds";
        
        public async UniTaskVoid Play(string audioClipName)
        {
            var audioClip = await GetAudio(audioClipName, AudioType);
            m_AudioSource.PlayOneShot(audioClip);
        }
    }
}