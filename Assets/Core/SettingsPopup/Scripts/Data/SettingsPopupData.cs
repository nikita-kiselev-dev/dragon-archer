using System;
using Core.SaveLoad;
using MemoryPack;

namespace Core.SettingsPopup.Scripts.Data
{
    [MemoryPackable]
    public partial class SettingsPopupData : SaveLoad.Data
    {
        [DataProperty] public float SoundsVolume { get; private set; }
        [DataProperty] public float MusicVolume { get; private set; }

        public override void PrepareNewData()
        {
            SoundsVolume = SettingsConstants.DefaultSoundsVolume;
            MusicVolume = SettingsConstants.DefaultMusicVolume;
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