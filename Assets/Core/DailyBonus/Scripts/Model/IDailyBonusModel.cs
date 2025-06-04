using System;
using System.Collections.Generic;
using Core.DailyBonus.Scripts.Dto;

namespace Core.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        IReadOnlyList<DailyBonusDayDto> DayConfigs { get; }
        int StreakDay { get; }
        DateTime LastRewardDate { get; }

        bool TryGetCurrentDayConfig(out DailyBonusDayDto config);
        bool IsTodayRewardReceived(DateTime serverTime);
        void AddStreakDay();
        void SetLastRewardDate(DateTime rewardDate);
        void ResetStreak();
        bool HasCollectedAllRewards();
    }
}