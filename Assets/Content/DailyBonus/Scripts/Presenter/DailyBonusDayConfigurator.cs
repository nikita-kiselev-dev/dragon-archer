﻿using System.Collections.Generic;
using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Factory;
using Content.DailyBonus.Scripts.Model;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusDayConfigurator : IDailyBonusDayConfigurator
    {
        private readonly IDailyBonusDto _dto;
        private readonly IDailyBonusModel _model;
        private readonly IRewardRowsManager _rewardRowsManager;
        private readonly IAssetLoader _assetLoader;

        public DailyBonusDayConfigurator(IDailyBonusDto dto, IDailyBonusModel model, IRewardRowsManager rewardRowsManager, IAssetLoader assetLoader)
        {
            _dto = dto;
            _model = model;
            _rewardRowsManager = rewardRowsManager;
            _assetLoader = assetLoader;
        }

        public List<IDailyBonusDayController> GetConfiguredDayControllers()
        {
            var dayConfigs = CreateDayConfigs();
            
            var dayFactory = new DailyBonusDayFactory(_assetLoader);
            var dayControllers = new List<IDailyBonusDayController>();
            
            foreach (var dayConfig in dayConfigs)
            {
                var dayView = dayFactory.CreateDayView(dayConfig);
                dayControllers.Add(new DailyBonusDayController(dayView, dayConfig));
            }

            return dayControllers;
        }
        
        private List<IDailyBonusDayConfig> CreateDayConfigs()
        {
            var dayConfigs = new List<IDailyBonusDayConfig>();
            var currentStreakDay = _model.GetStreakDay();
            var config = _dto.GetDays();
            
            for (var index = 0; index < config.Count; index++)
            {
                var isLastDay = index == config.Count - 1;
                
                var dayParent = isLastDay ? 
                    _rewardRowsManager.GetLastRewardParent() : 
                    _rewardRowsManager.GetRewardParent(index, config.Count - 1);
                
                var dayDto = config[index];
                var dayType = GetDayType(dayDto.StreakDay, currentStreakDay, isLastDay);
                var itemSprite = _assetLoader.LoadAsset<Sprite>(dayDto.ItemSprite).Result;
                
                var dayConfig = new DailyBonusDayConfig(
                    dayType,
                    dayDto.StreakDay, 
                    dayDto.ItemName,
                    dayDto.ItemCount,
                    itemSprite,
                    dayParent);
                
                dayConfigs.Add(dayConfig);
            }

            return dayConfigs;
        }
        
        private string GetDayType(int streakDay, int currentStreakDay, bool isLastDay)
        {
            if (currentStreakDay > streakDay)
            {
                return DailyBonusInfo.DailyBonusPreviousDay;
            }
            else if (currentStreakDay < streakDay && isLastDay)
            {
                return DailyBonusInfo.DailyBonusLastDay;
            }
            else if (currentStreakDay < streakDay)
            {
                return DailyBonusInfo.DailyBonusNextDay;
            }
            else if (isLastDay)
            {
                return DailyBonusInfo.DailyBonusTodayLastDay;
            }
            else
            {
                return DailyBonusInfo.DailyBonusToday;
            }
        }
    }
}