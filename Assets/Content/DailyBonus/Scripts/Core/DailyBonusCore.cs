using System;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Content.Items.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.LiveOps;

namespace Content.DailyBonus.Scripts.Core
{
    public class DailyBonusCore : IDailyBonusCore
    {
        private readonly IDailyBonusModel _model;
        private readonly IDailyBonusAnalytics _analytics;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;

        public DailyBonusCore(
            IDailyBonusModel model, 
            IDailyBonusAnalytics analytics,
            IServerTimeService serverTimeService,
            IInventoryManager inventoryManager)
        {
            _model = model;
            _analytics = analytics;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }
        
        public async UniTask<bool> NeedToShowPopup()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var streakIsLoosed = StreakIsLoosed(serverTime);
            
            if (!streakIsLoosed && !_model.TodayRewardWasReceived)
            {
                _model.AddStreakDay();
            }
            
            var isFirstLaunch = await _model.IsFirstServerLaunch();
            
            if (streakIsLoosed && !isFirstLaunch)
            {
                _analytics.LogStreakLose(_model.StreakDay);
            }
            
            var areAllRewardsReceived = _model.HasCollectedAllRewards();
            
            if (streakIsLoosed || areAllRewardsReceived)
            {
                _model.ResetData();
                return true;
            }

            var todayIsRewardDay = _model.TodayIsRewardDay();
            var needToShowPopup = todayIsRewardDay && !_model.TodayRewardWasReceived;
            
            return needToShowPopup;
        }

        public void GiveReward()
        {
            var currentDayConfig = _model.GetDayConfig();
            var rewardResult = _inventoryManager.AddItem(currentDayConfig.ItemName, currentDayConfig.ItemCount);
            _model.SetTodayRewardStatus(rewardResult);
        }
        
        private bool StreakIsLoosed(DateTime serverTime)
        {
            var timeSinceLastServerSession = serverTime - _model.LastSessionServerTime;
            var daysSinceLastServerSession = timeSinceLastServerSession.Days;

            if (daysSinceLastServerSession == 1)
            {
                _model.SetTodayRewardStatus(false);
            }
            
            var streakIsLoosed = daysSinceLastServerSession > 1;
            return streakIsLoosed;
        }
    }
}