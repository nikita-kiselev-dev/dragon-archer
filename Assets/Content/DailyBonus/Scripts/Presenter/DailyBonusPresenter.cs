using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.View;
using Infrastructure.Service.Asset;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusPresenter : IDailyBonusPresenter
    {
        private readonly IDailyBonusModel _model;
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;
        private readonly IAssetLoader _assetLoader;
        private readonly IServerTimeService _serverTimeService;

        private IDailyBonusView _view;
        private IViewInteractor _viewInteractor;

        private List<IDailyBonusDayController> _days;

        public DailyBonusPresenter(
            IDailyBonusModel model,
            IViewFactory viewFactory,
            IViewManager viewManager,
            IAssetLoader assetLoader,
            IServerTimeService serverTimeService)
        {
            _model = model;
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _assetLoader = assetLoader;
            _serverTimeService = serverTimeService;
        }
        
        public async void Init()
        {
            var needToShowPopup = await NeedToShowPopup();

            if (!needToShowPopup)
            {
                //return;
            }
            
            RegisterAndInitView();
            ConfigureView();

            Open(); 
        }
        
        public void Open()
        {
            _viewInteractor.Open();
        }

        public void Close()
        {
            _viewInteractor.Close();
        }

        private async Task<bool> NeedToShowPopup()
        {
            var serverTime = await _serverTimeService.GetServerTime();
            var startStreakData = _model.GetStartStreakData();
            var timeSinceStartStreak = serverTime - startStreakData;
            var streakIsLoosed = timeSinceStartStreak.Hours > DailyBonusInfo.HoursToResetStreak;
            var isFirstLaunch = startStreakData == DateTime.UnixEpoch;
            
            if (streakIsLoosed || isFirstLaunch)
            {
                _model.ResetData(serverTime);
                return true;
            }

            var addStreakDay = timeSinceStartStreak.Hours 
                is > DailyBonusInfo.MinHoursToGetReward and < DailyBonusInfo.HoursToResetStreak;

            if (!addStreakDay)
            {
                return false;
            }
            
            _model.AddStreakDay();
            return true;
        }
        
        private void RegisterAndInitView()
        {
            _view = _viewFactory.CreateView<IDailyBonusView>(ViewInfo.DailyBonus, ViewType.Popup);
            
            var viewSignalManager = new ViewSignalManager()
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.DailyBonus)
                .SetViewType(ViewType.Popup)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .RegisterAndInit();
        }

        private void ConfigureView()
        {
            //TODO: get info from server
            //TODO: read rewards config
            //TODO: recode method
            
            var dailyBonusDays = 7;
            
            _days = new List<IDailyBonusDayController>();
            
            for (var index = 0; index < dailyBonusDays; index++)
            {
                var rewardParent = _view.RewardRowsManager.GetRewardParent(index, dailyBonusDays);
                _days.Add(new DailyBonusDayController(_assetLoader, rewardParent));
            }
            
            for (var index = 0; index < _days.Count; index++)
            {
                var day = _days[index];
                day.Init();
            }
        }
    }
}