using System;
using Infrastructure.Service.SaveLoad;
using MemoryPack;

namespace Content.DailyBonus.Scripts.Data
{
    [MemoryPackable]
    public partial class DailyBonusData : Infrastructure.Service.SaveLoad.Data
    {
        [DataProperty] public int StreakDay { get; private set; }
        [DataProperty] public bool TodayRewardWasReceived { get; private set; }
        [DataProperty] public DateTime LastRewardDate { get; private set; }
        
        public override void PrepareNewData()
        {
            StreakDay = 0;
            TodayRewardWasReceived = false;
        }

        public void ResetStreak()
        {
            StreakDay = 1;
            TodayRewardWasReceived = false;
        }

        public void AddStreakDayData()
        {
            StreakDay++;
        }

        public void SetTodayRewardStatus(bool isReceived)
        {
            TodayRewardWasReceived = isReceived;
        }

        public void SetLastRewardDate(DateTime date)
        {
            LastRewardDate = date;
        }
    }
}