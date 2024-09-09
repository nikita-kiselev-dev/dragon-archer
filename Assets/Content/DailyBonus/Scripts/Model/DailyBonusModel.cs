using System;
using System.Collections.Generic;
using Content.DailyBonus.Scripts.Data;
using Content.DailyBonus.Scripts.Dto;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Data;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Model
{
    public class DailyBonusModel : IDailyBonusModel
    {
        private readonly DailyBonusData _dailyBonusData;
        private readonly IDailyBonusDto _dto;
        private readonly IMainDataManager _mainDataManager;
        
        public IReadOnlyList<DailyBonusDayDto> DayConfigs => _dto.GetDays();
        public int StreakDay => _dailyBonusData.StreakDay;
        public bool TodayRewardWasReceived => _dailyBonusData.TodayRewardWasReceived;
        public DateTime LastSessionServerTime => _mainDataManager.LastSessionServerTime;
        
        public DailyBonusModel(IDailyBonusDto dto, DailyBonusData dailyBonusData, IMainDataManager mainDataManager)
        {
            _dto = dto;
            _dailyBonusData = dailyBonusData;
            _mainDataManager = mainDataManager;
        }
        
        public DailyBonusDayDto GetDayConfig() => _dto.GetDay(StreakDay);
        public void AddStreakDay() => _dailyBonusData.AddStreakDayData();
        public bool TodayIsRewardDay() => _dto.GetDay(StreakDay) != null;
        public void SetTodayRewardStatus(bool isReceived) => _dailyBonusData.SetTodayRewardStatus(isReceived);
        public void ResetData() => _dailyBonusData.ResetStreak();
        public async UniTask<bool> IsFirstServerLaunch() => await _mainDataManager.IsFirstServerLaunch();
        
        public bool HasCollectedAllRewards()
        {
            var lastDayConfig = _dto.GetLastDay();
            
            if (lastDayConfig == null)
            {
                Debug.LogError($"{GetType().Name}: last day dto config does not exists!");
                return true;
            }
            
            var lastStreakDayInDto = lastDayConfig.StreakDay;
            var isAllRewardsReceived = StreakDay > lastStreakDayInDto;
            return isAllRewardsReceived;
        }
    }
}