using Content.DailyBonus.Scripts.Core;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.View;
using Content.Items.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using Infrastructure.Service.View.ViewSignalManager;

namespace Content.DailyBonus.Scripts.Presenter
{
    public class DailyBonusPresenter : IDailyBonusPresenter
    {
        private readonly IDailyBonusAnalytics _analytics;
        private readonly IDailyBonusModel _model;
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;
        private readonly IAssetLoader _assetLoader;
        private readonly IServerTimeService _serverTimeService;
        private readonly IInventoryManager _inventoryManager;
        
        private IDailyBonusCore _core;
        private IDailyBonusView _view;
        private IViewInteractor _viewInteractor;
        
        public bool IsInited { get; private set; }

        public DailyBonusPresenter(
            IDailyBonusAnalytics analytics,
            IDailyBonusModel model,
            IViewFactory viewFactory,
            IViewManager viewManager,
            IAssetLoader assetLoader,
            IServerTimeService serverTimeService,
            IInventoryManager inventoryManager)
        {
            _analytics = analytics;
            _model = model;
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _assetLoader = assetLoader;
            _serverTimeService = serverTimeService;
            _inventoryManager = inventoryManager;
        }

        public async UniTaskVoid Init()
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
                return;
            }
            
            await RegisterAndInitView();
            await CreateDays();
            IsInited = true;
            Open();
            _core.GiveReward();
        }
        
        private void Open()
        {
            _viewInteractor.Open();
            var streakDay = _model.StreakDay;
            _analytics.LogPopupOpen(streakDay);
        }

        private void Close()
        {
            _viewInteractor.Close();
        }
        
        private async UniTask RegisterAndInitView()
        {
            _view = await _viewFactory.CreateView<IDailyBonusView>(DailyBonusInfo.Popup, ViewType.Popup);
            
            var viewSignalManager = new ViewSignalManager()
                .AddCloseSignal(Close);
            
            _viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(ViewInfo.DailyBonus)
                .SetViewType(ViewType.Popup)
                .SetView(_view)
                .SetViewSignalManager(viewSignalManager)
                .SetAfterCloseAction(AfterCloseAction)
                .RegisterAndInit();
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

        private void AfterCloseAction()
        {
            _view.Destroy();
        }
    }
}