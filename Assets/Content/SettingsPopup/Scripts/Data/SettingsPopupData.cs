using System;
using Infrastructure.Service.SaveLoad;
using MemoryPack;

namespace Content.SettingsPopup.Scripts.Data
{
    [MemoryPackable]
    public partial class SettingsPopupData : Infrastructure.Service.SaveLoad.Data
    {
        [DataProperty] public float SoundsVolume { get; private set; }
        [DataProperty] public float MusicVolume { get; private set; }

        public override void PrepareNewData()
        {
            SoundsVolume = SettingsPopupInfo.DefaultSoundsVolume;
            MusicVolume = SettingsPopupInfo.DefaultMusicVolume;
        }

        public void SetSoundsVolumeData(float value)
        {
            var roundedValue = Math.Round(value, 2);
            SoundsVolume = (float)roundedValue;
        }

        public void SetMusicVolumeData(float value)
        {
            var roundedValue = Math.Round(value, 2);
            MusicVolume = (float)roundedValue;
        }
    }
}