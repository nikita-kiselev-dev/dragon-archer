using Content.SettingsPopup.Scripts.Data;
using Content.SettingsPopup.Scripts.Model;
using Content.SettingsPopup.Scripts.Presenter;
using Content.SettingsPopup.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.Decorators.FastView;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.SettingsPopup.Scripts
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.SettingsPopup)]
    [FastViewDecoratable]
    public class SettingsCore : ControlEntity, ISettings
    {
        [Inject] private readonly SettingsPopupData _data;
        
        [FastView(SettingsConstants.PopupKey, ViewType.Popup)] private SettingsPopupView _view;
        private ISettingsPopupModel _model;
        private ISettingsPopupPresenter _presenter;

        bool ISettings.IsInited => _presenter.IsInited;
        
        protected override UniTask Init()
        {
            CreateModel();
            CreatePresenter();
            _presenter.Init();
            return UniTask.CompletedTask;
        }
        
        void ISettings.OpenPopup()
        {
            _presenter.Open();
        }

        private void CreateModel()
        {
            _model = new SettingsPopupModel(_data);
        }

        private void CreatePresenter()
        {
            _presenter = new SettingsPopupPresenter(_model, _view);
        }
    }
}