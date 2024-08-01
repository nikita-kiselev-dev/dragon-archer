using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Data
{
    [Serializable]
    [JsonObject(MemberSerialization.Fields)]
    public class DailyBonusData : Infrastructure.Service.SaveLoad.Data
    {
        [SerializeField] private int m_StreakDay;
        [SerializeField] private long m_StartStreakDate;

        public int StreakDay => m_StreakDay;
        public long StartStreakDate => m_StartStreakDate;
        
        public override void WhenDataIsNew()
        {
            m_StreakDay = 0;
            m_StartStreakDate = 0;
        }

        public void ResetStreak()
        {
            m_StreakDay = 1;
            m_StartStreakDate = 0;
        }

        public void AddStreakDayData()
        {
            m_StreakDay++;
        }

        public void SetStartStreakDateData(long date)
        {
            m_StartStreakDate = date;
        }
    }
}