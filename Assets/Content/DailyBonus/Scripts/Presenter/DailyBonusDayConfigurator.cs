using System.Collections.Generic;
using Content.DailyBonus.Scripts.Config;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Factory;
using Content.DailyBonus.Scripts.Model;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View;
using UnityEngine;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusDayConfigurator : IDailyBonusDayConfigurator
    {
        private readonly IDailyBonusModel _model;
        private readonly IRewardRowsManager _rewardRowsManager;
        private readonly IAssetLoader _assetLoader;

        public DailyBonusDayConfigurator(
            IDailyBonusModel model, 
            IRewardRowsManager rewardRowsManager, 
            IAssetLoader assetLoader)
        {
            _model = model;
            _rewardRowsManager = rewardRowsManager;
            _assetLoader = assetLoader;
        }

        public async UniTask<List<IDailyBonusDayController>> GetConfiguredDayControllers()
        {
            var dayConfigs = await CreateDayConfigs();
            
            var dayFactory = new DailyBonusDayFactory(_assetLoader);
            var dayControllers = new List<IDailyBonusDayController>();
            
            foreach (var dayConfig in dayConfigs)
            {
                var dayView = await dayFactory.CreateDayView(dayConfig);
                dayControllers.Add(new DailyBonusDayController(dayView, dayConfig));
            }

            return dayControllers;
        }
        
        private async UniTask<List<IDailyBonusDayConfig>> CreateDayConfigs()
        {
            var dayConfigs = new List<IDailyBonusDayConfig>();
            var currentStreakDay = _model.StreakDay;
            var config = _model.DayConfigs;
            
            for (var index = 0; index < config.Count; index++)
            {
                var dayConfig = await CreateDayConfig(index, config.Count, currentStreakDay, config[index]);
                dayConfigs.Add(dayConfig);
            }

            return dayConfigs;
        }

        private async UniTask<IDailyBonusDayConfig> CreateDayConfig(
            int index,
            int configCount,
            int currentStreakDay,
            DailyBonusDayDto dayDto)
        {
            var isLastDay = index == configCount- 1;
                
            var dayParent = isLastDay ? 
                _rewardRowsManager.GetLastRewardParent() : 
                _rewardRowsManager.GetRewardParent(index, configCount - 1);
            
            var dayType = GetDayType(dayDto.StreakDay, currentStreakDay, isLastDay);
            var itemSprite = await _assetLoader.LoadAssetAsync<Sprite>(dayDto.ItemSprite);
                
            var dayConfig = new DailyBonusDayConfig(
                dayType,
                dayDto.StreakDay, 
                dayDto.ItemName,
                dayDto.ItemCount,
                itemSprite,
                dayParent);

            return dayConfig;
        }
        
        private string GetDayType(int streakDay, int currentStreakDay, bool isLastDay)
        {
            if (currentStreakDay > streakDay)
            {
                return DailyBonusInfo.PreviousDay;
            }
            else if (currentStreakDay < streakDay && isLastDay)
            {
                return DailyBonusInfo.LastDay;
            }
            else if (currentStreakDay < streakDay)
            {
                return DailyBonusInfo.NextDay;
            }
            else if (isLastDay)
            {
                return DailyBonusInfo.TodayLastDay;
            }
            else
            {
                return DailyBonusInfo.Today;
            }
        }
    }
}