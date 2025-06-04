using Core.SettingsPopup.Scripts.Data;

namespace Core.SettingsPopup.Scripts.Model
{
    public class SettingsPopupModel : ISettingsPopupModel
    {
        private readonly SettingsPopupData _data;
        
        public SettingsPopupModel(SettingsPopupData data)
        {
            _data = data;
        }
        
        public float GetSoundsVolume()
        {
            return _data.SoundsVolume;
        }

        public float GetMusicVolume()
        {
            return _data.MusicVolume;
        }

        public void SetSoundsVolume(float volume)
        {
            _data.SetSoundsVolumeData(volume);
        }

        public void SetMusicVolume(float volume)
        {
            _data.SetMusicVolumeData(volume);
        }
    }
}