using System;
using Infrastructure.Service.SaveLoad;
using MemoryPack;

namespace Content.DailyBonus.Scripts.Data
{
    [MemoryPackable]
    public partial class DailyBonusData : Infrastructure.Service.SaveLoad.Data
    {
        [DataProperty] public int StreakDay { get; private set; }
        [DataProperty] public DateTime StartStreakDate { get; private set; }
        
        public override void PrepareNewData()
        {
            StreakDay = 0;
            StartStreakDate = DateTime.UnixEpoch;
        }

        public bool IsFirstLaunch()
        {
            return StartStreakDate == DateTime.UnixEpoch;
        }

        public void ResetStreak(DateTime startStreakDate)
        {
            StreakDay = 1;
            StartStreakDate = startStreakDate;
        }

        public void AddStreakDayData()
        {
            StreakDay++;
        }
    }
}