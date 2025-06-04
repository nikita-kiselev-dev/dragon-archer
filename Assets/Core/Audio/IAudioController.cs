namespace Core.Audio
{
    public interface IAudioController
    {
        public void PlaySound(string audioClipName);
        public void SetSoundsVolume(float volume);
        public void PlayMusic(string audioClipName);
        public void SetMusicVolume(float volume);
    }
}