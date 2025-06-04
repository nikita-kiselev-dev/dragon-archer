using System;
using System.Collections.Generic;
using Core.DailyBonus.Scripts.Data;
using Core.DailyBonus.Scripts.Dto;
using Core.Logger;

namespace Core.DailyBonus.Scripts.Model
{
    public class DailyBonusModel : IDailyBonusModel
    {
        private readonly DailyBonusData _dailyBonusData;
        private readonly IDailyBonusDto _dto;
        private readonly ILogManager _logger = new LogManager(nameof(DailyBonusModel));
        
        public IReadOnlyList<DailyBonusDayDto> DayConfigs => _dto.GetDays();
        public int StreakDay => _dailyBonusData.StreakDay;
        public DateTime LastRewardDate => _dailyBonusData.LastRewardDate;
        
        public DailyBonusModel(IDailyBonusDto dto, DailyBonusData dailyBonusData)
        {
            _dto = dto;
            _dailyBonusData = dailyBonusData;
        }
        
        public bool TryGetCurrentDayConfig(out DailyBonusDayDto config)
        {
            config = _dto.GetDay(StreakDay);
            return config != null;
        }

        public bool IsTodayRewardReceived(DateTime serverTime) 
        {
            return serverTime.Date == _dailyBonusData.LastRewardDate.Date;
        }

        public void AddStreakDay()
        {
            _dailyBonusData.AddStreakDayData();
        }

        public void SetLastRewardDate(DateTime rewardDate)
        {
            _dailyBonusData.SetLastRewardDate(rewardDate);
        }

        public void ResetStreak()
        {
            _dailyBonusData.ResetStreak();
        }

        public bool HasCollectedAllRewards()
        {
            var lastDayConfig = _dto.GetLastDay();
            
            if (lastDayConfig == null)
            {
                _logger.LogError("Last day dto config does not exists!");
                return true;
            }
            
            var lastStreakDayInDto = lastDayConfig.StreakDay;
            var isAllRewardsReceived = StreakDay > lastStreakDayInDto;
            return isAllRewardsReceived;
        }
    }
}