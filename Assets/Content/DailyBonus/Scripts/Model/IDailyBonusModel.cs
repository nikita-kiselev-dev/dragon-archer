using System;
using System.Collections.Generic;
using Content.DailyBonus.Scripts.Dto;
using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        public int StreakDay { get; }
        public IReadOnlyList<DailyBonusDayDto> DayConfigs { get; }
        public DateTime LastSessionServerTime { get; }
        public bool TodayRewardWasReceived { get; }

        public void SetTodayRewardStatus(bool isReceived);
        public UniTask<bool> IsFirstServerLaunch();
        public void AddStreakDay();
        public bool AreAllRewardsReceived();
        public bool TodayIsRewardDay();
        public DailyBonusDayDto GetDayConfig();
        public void ResetData();
    }
}