using Content.DailyBonus.Scripts.Data;
using Content.DailyBonus.Scripts.Dto;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Content.Items.Scripts;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Date;
using Infrastructure.Service.Dto;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.DailyBonus.Scripts
{
    public class DailyBonus : IDailyBonus
    {
        [Inject] private readonly IDtoReader _dtoReader;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly IServerTimeService _serverTimeService;
        [Inject] private readonly IInventoryManager _inventoryManager;
        
        [Inject] private readonly DailyBonusData _data;
        [Inject] private readonly IDateConverter _dateConverter;
        
        private IDailyBonusModel _model;
        private IDailyBonusPresenter _presenter;
        
        public void Init()
        {
            CreateModel();
            CreatePresenter();
            _presenter.Init();
        }

        private void CreateModel()
        {
            _model = new DailyBonusModel(_data, _dateConverter);
        }
        
        private void CreatePresenter()
        {
            var dto = _dtoReader.Read<DailyBonusDto>(DailyBonusInfo.DailyBonusConfig);
            
            _presenter = new DailyBonusPresenter(
                    dto, 
                    _model, 
                    _viewFactory, 
                    _viewManager, 
                    _assetLoader, 
                    _serverTimeService, 
                    _inventoryManager);
        }
    }
}