using Content.DailyBonus.Scripts.Core;
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

        private IDailyBonusCore _core;
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
        
        public void Init()
        {
            _core = new DailyBonusCore(_dto, _model, _serverTimeService, _inventoryManager);
            var needToShowPopup = _core.NeedToShowPopup();

            if (!needToShowPopup)
            {
                //return;
            }
            
            RegisterAndInitView();
            CreateDays();
            Open();
            _core.GetStreakReward();
        }
        
        public void Open()
        {
            _viewInteractor.Open();
        }

        public void Close()
        {
            _viewInteractor.Close();
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