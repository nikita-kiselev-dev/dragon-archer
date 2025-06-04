namespace Core.SettingsPopup.Scripts.Model
{
    public interface ISettingsPopupModel
    {
        public float GetSoundsVolume();
        public float GetMusicVolume();
        public void SetSoundsVolume(float volume);
        public void SetMusicVolume(float volume);
    }
}