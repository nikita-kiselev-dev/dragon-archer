using System;
using Core.DailyBonus.Scripts.Model;
using Core.DailyBonus.Scripts.Presenter;
using Core.Items.Scripts;
using Core.LiveOps;
using Cysharp.Threading.Tasks;

namespace Core.DailyBonus.Scripts.Core
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

            if (_model.LastRewardDate == default)
            {
                _model.SetLastRewardDate(serverTime - TimeSpan.FromDays(1));
            }
            
            return ShouldShowPopup(serverTime) || ProcessStreakUpdate(serverTime);
        }

        private bool ProcessStreakUpdate(DateTime serverTime)
        {
            var daysPassed = (serverTime.Date - _model.LastRewardDate.Date).Days;

            if (daysPassed > 1)
            {
                _analytics.LogStreakLose(_model.StreakDay);
                _model.ResetStreak();
                return true;
            }

            if (daysPassed == 1)
            {
                _model.AddStreakDay();
                _model.SetLastRewardDate(serverTime);
                return true;
            }

            if (_model.HasCollectedAllRewards())
            {
                _model.ResetStreak();
                return true; 
            }

            return false;
        }


        private bool ShouldShowPopup(DateTime serverTime)
        {
            return _model.TryGetCurrentDayConfig(out var config) && !_model.IsTodayRewardReceived(serverTime);
        }

        public async UniTask GiveReward()
        {
            if (!_model.TryGetCurrentDayConfig(out var config)) return;
            var rewardResult = _inventoryManager.AddItem(config.ItemName, config.ItemCount);
            if (!rewardResult) return;
            var serverTime = await _serverTimeService.GetServerTime();
            _model.SetLastRewardDate(serverTime);
        }
    }
}