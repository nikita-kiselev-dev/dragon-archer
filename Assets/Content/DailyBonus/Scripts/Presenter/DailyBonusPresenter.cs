using System;
using System.Linq;
using System.Threading.Tasks;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.View;
using Content.Items.Scripts;
using Infrastructure.Service.Asset;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusPresenter : IDailyBonusPresenter
    {
        private readonly IDailyBonusDto _dto;
        private readonly IDailyBonusModel _model;
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;
        private readonly IAssetLoader _assetLoader;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;

        private IDailyBonusView _view;
        private IViewInteractor _viewInteractor;

        public DailyBonusPresenter(
            IDailyBonusDto dto,
            IDailyBonusModel model,
            IViewFactory viewFactory,
            IViewManager viewManager,
            IAssetLoader assetLoader,
            IServerTimeService serverTimeService,
            IInventoryManager inventoryManager)
        {
            _dto = dto;
            _model = model;
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _assetLoader = assetLoader;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }
        
        public async void Init()
        {
            var needToShowPopup = await NeedToShowPopup();

            if (!needToShowPopup)
            {
                //return;
            }
            
            RegisterAndInitView();
            CreateDays();
            GetItems();
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

            var streakIsLoosed = StreakIsLoosed(timeSinceStartStreak);
            var isFirstLaunch = IsFirstLaunch(startStreakData);
            var areAllRewardsReceived = AreAllRewardsReceived();
            
            if (streakIsLoosed || isFirstLaunch || areAllRewardsReceived)
            {
                _model.ResetData(serverTime);
                return true;
            }

            var addStreakDay = timeSinceStartStreak.TotalSeconds 
                is > DailyBonusInfo.MinSecondsToGetReward and < DailyBonusInfo.SecondsToResetStreak;

            if (addStreakDay)
            {
                _model.AddStreakDay();
                return true;
            }

            return false;
        }

        private bool StreakIsLoosed(TimeSpan timeSinceStartStreak)
        {
            var streakIsLoosed = timeSinceStartStreak.TotalSeconds > DailyBonusInfo.SecondsToResetStreak;
            return streakIsLoosed;
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
        
        private void CreateDays()
        {
            var dayConfigurator = new DailyBonusDayConfigurator(_dto, _model, _view.RewardRowsManager, _assetLoader);
            var dayControllers = dayConfigurator.GetConfiguredDayControllers();

            foreach (var dayController in dayControllers)
            {
                dayController.Init();
            }
        }

        private void GetItems()
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
        
        private void RegisterAndInitView()
        {
            _view = _viewFactory.CreateView<IDailyBonusView>(DailyBonusInfo.DailyBonusPopup, ViewType.Popup);
            
            var viewSignalManager = new ViewSignalManager()
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.DailyBonus)
                .SetViewType(ViewType.Popup)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .RegisterAndInit();
        }
    }
}