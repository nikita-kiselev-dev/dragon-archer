using Content.Settings.Scripts.Data;
using Content.Settings.Scripts.Model;
using Content.Settings.Scripts.Presenter;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.Settings.Scripts
{
    public class SettingsPopup : ISettingsPopup
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly SettingsPopupData _data;
        
        private ISettingsPopupModel _model;
        private ISettingsPopupPresenter _presenter;

        public bool IsInited => _presenter.IsInited;
        
        public void Init()
        {
            CreateModel();
            CreatePresenter();
            _presenter.Init();
        }

        private void CreateModel()
        {
            _model = new SettingsPopupModel(_data);
        }

        private void CreatePresenter()
        {
            _presenter = new SettingsPopupPresenter(_model, _viewFactory, _viewManager);
        }
    }
}