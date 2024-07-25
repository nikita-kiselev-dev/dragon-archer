using Content.DailyBonus.Scripts.Data;
using Content.DailyBonus.Scripts.Model;
using Content.DailyBonus.Scripts.Presenter;
using Infrastructure.Service.Date;
using Infrastructure.Service.LiveOps;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.DailyBonus.Scripts
{
    public class DailyBonus : IDailyBonus
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IServerTimeService _serverTimeService;
        
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
            _presenter = new DailyBonusPresenter(_model, _viewFactory, _viewManager, _serverTimeService);
        }
    }
}