namespace Infrastructure.Service.Audio
{
    public interface IAudioService
    {
        public void PlaySound(string audioClipName);
        public void SetSoundsVolume(float volume);
        public void PlayMusic(string audioClipName);
        public void SetMusicVolume(float volume);
    }
}