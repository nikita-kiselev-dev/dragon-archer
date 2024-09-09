using System;
using System.Collections.Generic;
using Content.DailyBonus.Scripts.Dto;
using Cysharp.Threading.Tasks;

namespace Content.DailyBonus.Scripts.Model
{
    public interface IDailyBonusModel
    {
        public IReadOnlyList<DailyBonusDayDto> DayConfigs { get; }
        public int StreakDay { get; }
        public bool TodayRewardWasReceived { get; }
        public DateTime LastSessionServerTime { get; }

        public DailyBonusDayDto GetDayConfig();
        public void AddStreakDay();
        public bool TodayIsRewardDay();
        public void SetTodayRewardStatus(bool isReceived);
        public void ResetData();
        public UniTask<bool> IsFirstServerLaunch();
        public bool HasCollectedAllRewards();
    }
}