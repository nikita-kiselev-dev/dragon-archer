using System;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Content.Items.Common.Scripts;
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
            return ProcessStreakUpdate(serverTime) || ShouldShowPopup(serverTime);
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