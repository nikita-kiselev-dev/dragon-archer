﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Model;
using Content.Items.Scripts;
using Infrastructure.Service.LiveOps;

namespace Content.DailyBonus.Scripts.Core
{
    public class DailyBonusCore : IDailyBonusCore
    {
        private readonly IDailyBonusDto _dto;
        private readonly IDailyBonusModel _model;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;

        public DailyBonusCore(
            IDailyBonusDto dto, 
            IDailyBonusModel model, 
            IServerTimeService serverTimeService, 
            IInventoryManager inventoryManager)
        {
            _dto = dto;
            _model = model;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }
        
        public async Task<bool> NeedToShowPopup()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var startStreakData = _model.GetStartStreakData();
            var timeSinceStartStreak = serverTime - startStreakData;

            var streakIsLoosed = StreakIsLoosed(timeSinceStartStreak);
            var isFirstLaunch = IsFirstLaunch(startStreakData);
            var areAllRewardsReceived = AreAllRewardsReceived();
            
            if (streakIsLoosed || isFirstLaunch || areAllRewardsReceived)
            {
                _model.ResetData(serverTime);
                return true;
            }

            var addStreakDay = timeSinceStartStreak.TotalSeconds 
                is > DailyBonusInfo.MinSecondsToGetReward and < DailyBonusInfo.SecondsToResetStreak;

            if (addStreakDay)
            {
                _model.AddStreakDay();
                return true;
            }

            return false;
        }
        
        public void GetStreakReward()
        {
            var streakDay = _model.GetStreakDay();
            var config = _dto.GetDays();
            
            foreach (var dayConfig in config)
            {
                if (dayConfig.StreakDay == streakDay)
                {
                    _inventoryManager.AddItem(dayConfig.ItemName, dayConfig.ItemCount);
                }
            }
        }
        
        private bool StreakIsLoosed(TimeSpan timeSinceStartStreak)
        {
            var streakIsLoosed = timeSinceStartStreak.TotalSeconds > DailyBonusInfo.SecondsToResetStreak;
            return streakIsLoosed;
        }

        private bool IsFirstLaunch(DateTime startStreakData)
        {
            var isFirstLaunch = startStreakData == DateTime.UnixEpoch;
            return isFirstLaunch;
        }

        private bool AreAllRewardsReceived()
        {
            var currentStreakDay = _model.GetStreakDay();
            var config = _dto.GetDays();
            var lastStreakDayInDto = config.Last().StreakDay;
            var isAllRewardsReceived = currentStreakDay > lastStreakDayInDto;
            return isAllRewardsReceived;
        }
    }
}