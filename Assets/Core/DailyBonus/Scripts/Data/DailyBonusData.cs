using System;
using Core.SaveLoad;
using MemoryPack;

namespace Core.DailyBonus.Scripts.Data
{
    [MemoryPackable]
    public partial class DailyBonusData : global::Core.SaveLoad.Data
    {
        [DataProperty] public int StreakDay { get; private set; }
        [DataProperty] public DateTime LastRewardDate { get; private set; }
        
        public override void PrepareNewData()
        {
            StreakDay = 1;
            LastRewardDate = DateTime.Today - TimeSpan.FromDays(1);
        }

        public void ResetStreak()
        {
            StreakDay = 1;
        }

        public void AddStreakDayData()
        {
            StreakDay++;
        }

        public void SetLastRewardDate(DateTime date)
        {
            LastRewardDate = date;
        }
    }
}