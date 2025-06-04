

using Core.Analytics;
using Core.Asset;
using Core.Asset.IconController;
using Core.Controller;
using Core.DailyBonus.Scripts.Data;
using Core.DailyBonus.Scripts.Dto;
using Core.DailyBonus.Scripts.Model;
using Core.DailyBonus.Scripts.Presenter;
using Core.DailyBonus.Scripts.View;
using Core.Dto;
using Core.Initialization.Scripts;
using Core.Initialization.Scripts.Decorators.FastView;
using Core.Initialization.Scripts.InitOrder;
using Core.Initialization.Scripts.Scopes;
using Core.Items.Scripts;
using Core.LiveOps;
using Core.Logger;
using Core.SaveLoad;
using Core.SignalBus;
using Core.View.ViewManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.DailyBonus.Scripts
{
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.DailyBonus)]
    [FastViewDecoratable]
    public class DailyBonus : ControlEntity, IDailyBonus
    {
        [Inject] private readonly ISignalBus _signalBus;
        [Inject] private readonly IDtoReader _dtoReader;
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly DailyBonusData _dailyBonusData;
        [Inject] private readonly IIconController _iconController;
        [Inject] private readonly IMainDataManager _mainDataManager;
        [Inject] private readonly IInventoryManager _inventoryManager;
        [Inject] private readonly IAnalyticsManager _analyticsManager;
        [Inject] private readonly IServerTimeService _serverTimeService;
        [Inject] private readonly IServerConnectionService _serverConnectionService;

        private readonly ILogManager _logger = new LogManager(nameof(DailyBonus));

        [FastView(DailyBonusConstants.Popup, ViewType.Popup, nameof(Unload))] 
        private DailyBonusView _view;
        private IDailyBonusDto _dto;
        private IDailyBonusModel _model;
        private IDailyBonusPresenter _presenter;

        bool IController.IsInited => _presenter.IsInited;
        bool IController.IsActive => _presenter.IsActive;

        protected override async UniTask Load()
        {
            if (!_serverConnectionService.IsConnectedToServer)
            {
                Unload();
                return;
            }
            
            _dto = await _dtoReader.Read<DailyBonusDto>(DailyBonusConstants.Config);
        }

        protected override async UniTask Init()
        {
            if (!IsLoadSucceed()) return;
            CreateModel();
            CreatePresenter();
            await _presenter.Init();
            if (!_presenter.IsActive) Unload();
            _logger.Log($"Init completed. Status: {_presenter.IsActive}.");
        }

        private bool IsLoadSucceed()
        {
            if (_dto != null) return true;
            _logger.Log("Init failed - no internet connection.");
            return false;
        }

        private void CreateModel()
        {
            _model = new DailyBonusModel(_dto, _dailyBonusData);
        }
        
        private void CreatePresenter()
        {
            var analytics = new DailyBonusAnalytics(_analyticsManager);
            
            _presenter = new DailyBonusPresenter(
                    analytics,
                    _view,
                    _model,
                    _assetLoader,
                    _iconController,
                    _serverTimeService,
                    _inventoryManager);
        }

        private void Unload()
        {
            if (_view)
            {
                _view.gameObject.SetActive(false);
                Object.Destroy(_view.gameObject);
            }
            
            _assetLoader.Release(DailyBonusConstants.Popup);
            _assetLoader.Release(DailyBonusConstants.PreviousDay);
            _assetLoader.Release(DailyBonusConstants.Today);
            _assetLoader.Release(DailyBonusConstants.NextDay);
            _assetLoader.Release(DailyBonusConstants.LastDay);
            _assetLoader.Release(DailyBonusConstants.TodayLastDay);
        }
    }
}