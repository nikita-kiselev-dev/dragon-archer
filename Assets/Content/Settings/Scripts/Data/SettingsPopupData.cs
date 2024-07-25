using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.Settings.Scripts.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class SettingsPopupData : Infrastructure.Service.SaveLoad.Data
    {
        [SerializeField] private float m_SoundsVolume;
        [SerializeField] private float m_MusicVolume;

        public float SoundsVolume => m_SoundsVolume;
        public float MusicVolume => m_MusicVolume;

        public override void WhenDataIsNew()
        {
            m_SoundsVolume = SettingsPopupInfo.DefaultSoundsVolume;
            m_MusicVolume = SettingsPopupInfo.DefaultMusicVolume;
        }

        public void SetSoundsVolumeData(float value)
        {
            var roundedValue = Math.Round(value, 2);
            m_SoundsVolume = (float)roundedValue;
        }

        public void SetMusicVolumeData(float value)
        {
            var roundedValue = Math.Round(value, 2);
            m_MusicVolume = (float)roundedValue;
        }
    }
}