using System.Collections.Generic;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Factory;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View;

namespace Content.DailyBonus.Scripts.Config
{
    public class DailyBonusDayConfigurator : IDailyBonusDayConfigurator
    {
        private readonly IDailyBonusDto _dto;
        private readonly IDailyBonusModel _model;
        private readonly RewardRowsManager _rewardRowsManager;
        private readonly IAssetLoader _assetLoader;

        public DailyBonusDayConfigurator(IDailyBonusDto dto, IDailyBonusModel model, RewardRowsManager rewardRowsManager, IAssetLoader assetLoader)
        {
            _model = model;
            _dto = dto;
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
            
            for (var index = 0; index < _dto.Days.Count; index++)
            {
                var dayParent = _rewardRowsManager.GetRewardParent(index, _dto.Days.Count);
                var dayDto = _dto.Days[index];
                var dayType = GetDayType(dayDto.StreakDay, currentStreakDay);
                
                var dayConfig = new DailyBonusDayConfig(
                    dayType,
                    dayDto.StreakDay, 
                    null,
                    dayDto.ItemCount,
                    dayParent);
                
                dayConfigs.Add(dayConfig);
            }

            return dayConfigs;
        }
        
        private string GetDayType(int streakDay, int currentStreakDay)
        {
            if (currentStreakDay > streakDay)
            {
                return DailyBonusInfo.DailyBonusPreviousDay;
            }
            else if (currentStreakDay < streakDay)
            {
                return DailyBonusInfo.DailyBonusNextDay;
            }
            else
            {
                return DailyBonusInfo.DailyBonusToday;
            }
        }
    }
}