using Content.DailyBonus.Scripts.Data;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Content.DailyBonus.Scripts.View;
using Content.Items.Scripts;
using Cysharp.Threading.Tasks;
using Infrastructure.Game.Data;
using Infrastructure.Service.Analytics;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Dto;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.Logger;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.DailyBonus.Scripts
{
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.DailyBonus)]
    public class DailyBonus : ControlEntity, IDailyBonus
    {
        [Inject] private readonly IDtoReader _dtoReader;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly DailyBonusData _dailyBonusData;
        [Inject] private readonly IMainDataManager _mainDataManager;
        [Inject] private readonly IInventoryManager _inventoryManager;
        [Inject] private readonly IAnalyticsManager _analyticsManager;
        [Inject] private readonly IServerTimeService _serverTimeService;
        [Inject] private readonly IServerConnectionService _serverConnectionService;

        private readonly ILogManager _logger = new LogManager(nameof(DailyBonus));

        private IDailyBonusDto _dto;
        private IDailyBonusView _view;
        private IDailyBonusModel _model;
        private IDailyBonusPresenter _presenter;

        public bool IsInited => _presenter.IsInited;

        protected override async UniTask Load()
        {
            if (!_serverConnectionService.IsConnectedToServer) return;
            
            _view = await _viewFactory.CreateView<IDailyBonusView>(DailyBonusInfo.Popup, ViewType.Popup);
            _dto = await _dtoReader.Read<DailyBonusDto>(DailyBonusInfo.Config);
        }

        protected override UniTask Init()
        {
            if (!IsLoadSucceed()) return UniTask.CompletedTask;
            
            CreateModel();
            CreatePresenter();
            _presenter.Init();
            _logger.Log("Init completed.");
            
            return UniTask.CompletedTask;
        }

        private bool IsLoadSucceed()
        {
            if (_view is not null && _dto is not null)
            {
                return true;
            }
            
            _logger.Log("Init failed - no internet connection.");
            return false;
        }

        private void CreateModel()
        {
            _model = new DailyBonusModel(_dto, _dailyBonusData, _mainDataManager);
        }
        
        private void CreatePresenter()
        {
            var analytics = new DailyBonusAnalytics(_analyticsManager);
            
            _presenter = new DailyBonusPresenter(
                    analytics,
                    _view,
                    _model,
                    _viewManager,
                    _assetLoader,
                    _serverTimeService,
                    _inventoryManager);
        }
    }
}