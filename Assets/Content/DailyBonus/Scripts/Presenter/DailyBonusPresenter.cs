using System;
using Content.DailyBonus.Scripts.Core;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.View;
using Content.Items.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.LiveOps;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusPresenter : IDailyBonusPresenter
    {
        private readonly IDailyBonusAnalytics _analytics;
        private readonly IDailyBonusView _view;
        private readonly IDailyBonusModel _model;
        private readonly IAssetLoader _assetLoader;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;
        
        private IDailyBonusCore _core;
        
        public bool IsInited { get; private set; }
        public bool IsActive { get; private set; }

        public DailyBonusPresenter(
            IDailyBonusAnalytics analytics,
            IDailyBonusView view,
            IDailyBonusModel model,
            IAssetLoader assetLoader,
            IServerTimeService serverTimeService,
            IInventoryManager inventoryManager)
        {
            _analytics = analytics;
            _view = view;
            _model = model;
            _assetLoader = assetLoader;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }

        public async UniTask Init()
        {
            _core = new DailyBonusCore(
                _model,
                _analytics,
                _serverTimeService,
                _inventoryManager);
            
            var needToShowPopup = await _core.NeedToShowPopup();

            if (!needToShowPopup)
            {
                IsInited = true;
                IsActive = false;
                return;
            }
            
            ConfigureView();
            await CreateDays();
            IsInited = true;
            IsActive = true;
            Open();
            _core.GiveReward();
        }

        private void ConfigureView()
        {
            _view.Init(Close);
        }
        
        private void Open()
        {
            _view.ViewInteractor.Open();
            var streakDay = _model.StreakDay;
            _analytics.LogPopupOpen(streakDay);
        }

        private void Close()
        {
            _view.ViewInteractor.Close();
            IsActive = false;
        }
        
        private async UniTask CreateDays()
        {
            var dayConfigurator = new DailyBonusDayConfigurator(_model, _view.RewardRowsManager, _assetLoader);
            var dayControllers = await dayConfigurator.GetConfiguredDayControllers();

            foreach (var dayController in dayControllers)
            {
                dayController.Init();
            }
        }
    }
}