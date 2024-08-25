using System;
using System.Linq;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Content.Items.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps;

namespace Content.DailyBonus.Scripts.Core
{
    public class DailyBonusCore : IDailyBonusCore
    {
        private readonly IDailyBonusDto _dto;
        private readonly IDailyBonusModel _model;
        private readonly IDailyBonusAnalytics _analytics;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;

        public DailyBonusCore(
            IDailyBonusDto dto, 
            IDailyBonusModel model, 
            IDailyBonusAnalytics analytics,
            IServerTimeService serverTimeService, 
            IInventoryManager inventoryManager)
        {
            _dto = dto;
            _model = model;
            _analytics = analytics;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }
        
        public async UniTask<bool> NeedToShowPopup()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var startStreakTime = _model.GetStartStreakTime();
            var streakIsLoosed = StreakIsLoosed(serverTime, startStreakTime);
            var isFirstLaunch = _model.IsFirstLaunch();
            
            if (streakIsLoosed && !isFirstLaunch)
            {
                _analytics.LogStreakLose(_model.GetStreakDay());
            }
            
            var areAllRewardsReceived = AreAllRewardsReceived();
            
            if (streakIsLoosed || isFirstLaunch || areAllRewardsReceived)
            {
                _model.ResetData(serverTime);
                return true;
            }

            if (streakIsLoosed)
            {
                return false;
            }
            
            _model.AddStreakDay();
            return true;
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
        
        private bool StreakIsLoosed(DateTime serverTime, DateTime startStreakTime)
        {
            var dayDifference = (serverTime - startStreakTime).Days;
            return dayDifference != 1;
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