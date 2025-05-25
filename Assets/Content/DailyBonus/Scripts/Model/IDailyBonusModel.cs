using System;
using System.Collections.Generic;
using Content.DailyBonus.Scripts.Dto;

namespace Content.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        public IReadOnlyList<DailyBonusDayDto> DayConfigs { get; }
        public int StreakDay { get; }
        public bool TodayRewardWasReceived { get; }
        public DateTime LastRewardDate { get; }

        public DailyBonusDayDto GetDayConfig();
        public void AddStreakDay();
        public bool TodayIsRewardDay();
        public void SetTodayRewardStatus(bool isReceived);
        public void SetLastRewardDate(DateTime rewardDate);
        public void ResetData();
        public bool HasCollectedAllRewards();
    }
}