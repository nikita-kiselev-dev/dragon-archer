using System.Collections.Generic;
using Core.Asset;
using Core.Asset.IconController;
using Core.DailyBonus.Scripts.Config;
using Core.DailyBonus.Scripts.Dto;
using Core.DailyBonus.Scripts.Factory;
using Core.DailyBonus.Scripts.Model;
using Core.View;
using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Presenter
{
    public class DailyBonusDayConfigurator : IDailyBonusDayConfigurator
    {
        private readonly IDailyBonusModel _model;
        private readonly IRewardRowsManager _rewardRowsManager;
        private readonly IAssetLoader _assetLoader;
        private readonly IIconController _iconController;

        public DailyBonusDayConfigurator(
            IDailyBonusModel model, 
            IRewardRowsManager rewardRowsManager, 
            IAssetLoader assetLoader,
            IIconController iconController)
        {
            _model = model;
            _rewardRowsManager = rewardRowsManager;
            _assetLoader = assetLoader;
            _iconController = iconController;
        }

        public async UniTask<List<IDailyBonusDayController>> GetConfiguredDayControllers()
        {
            var dayConfigs = await CreateDayConfigs();
            
            var dayFactory = new DailyBonusDayFactory(_assetLoader);
            var dayControllers = new List<IDailyBonusDayController>(dayConfigs.Count);
            
            foreach (var dayConfig in dayConfigs)
            {
                var dayView = await dayFactory.CreateDayView(dayConfig);
                dayControllers.Add(new DailyBonusDayController(dayView, dayConfig));
            }

            return dayControllers;
        }
        
        private async UniTask<List<IDailyBonusDayConfig>> CreateDayConfigs()
        {
            var dayConfigs = new List<IDailyBonusDayConfig>(_model.DayConfigs.Count);
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
            var itemSprite = await _iconController.GetIcon(dayDto.ItemSprite, dayDto.ItemName);
                
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
                return DailyBonusConstants.PreviousDay;
            }
            else if (currentStreakDay < streakDay && isLastDay)
            {
                return DailyBonusConstants.LastDay;
            }
            else if (currentStreakDay < streakDay)
            {
                return DailyBonusConstants.NextDay;
            }
            else if (isLastDay)
            {
                return DailyBonusConstants.TodayLastDay;
            }
            else
            {
                return DailyBonusConstants.Today;
            }
        }
    }
}