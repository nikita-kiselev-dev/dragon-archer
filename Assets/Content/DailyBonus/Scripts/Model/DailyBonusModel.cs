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
        
        public int StreakDay => _dailyBonusData.StreakDay;
        public IReadOnlyList<DailyBonusDayDto> DayConfigs => _dto.GetDays();
        public DateTime LastSessionServerTime => _mainDataManager.LastSessionServerTime;
        public bool TodayRewardWasReceived => _dailyBonusData.TodayRewardWasReceived;
        
        public DailyBonusModel(IDailyBonusDto dto, DailyBonusData dailyBonusData, IMainDataManager mainDataManager)
        {
            _dto = dto;
            _dailyBonusData = dailyBonusData;
            _mainDataManager = mainDataManager;
        }

        public async UniTask<bool> IsFirstServerLaunch()
        {
            return await _mainDataManager.IsFirstServerLaunch();
        }

        public void AddStreakDay()
        {
            _dailyBonusData.AddStreakDayData();
        }

        public void ResetData()
        {
            _dailyBonusData.ResetStreak();
        }

        public void SetTodayRewardStatus(bool isReceived)
        {
            _dailyBonusData.SetTodayRewardStatus(isReceived);
        }
        
        public int GetNextRewardDayIndex()
        {
            var nextStreakDay = _dto.GetNextDay(StreakDay).StreakDay;
            return nextStreakDay;
        }
        
        public bool AreAllRewardsReceived()
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

        public bool TodayIsRewardDay()
        {
            var dayConfig = _dto.GetDay(StreakDay);
            return dayConfig != null;
        }

        public DailyBonusDayDto GetDayConfig()
        {
            var dayConfig = _dto.GetDay(StreakDay);
            return dayConfig;
        }
    }
}