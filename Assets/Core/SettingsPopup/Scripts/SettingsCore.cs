using Core.Initialization.Scripts;
using Core.Initialization.Scripts.Decorators.FastView;
using Core.Initialization.Scripts.InitOrder;
using Core.Initialization.Scripts.Scopes;
using Core.SettingsPopup.Scripts.Data;
using Core.SettingsPopup.Scripts.Model;
using Core.SettingsPopup.Scripts.Presenter;
using Core.SettingsPopup.Scripts.View;
using Core.View.ViewManager;
using Cysharp.Threading.Tasks;
using VContainer;

namespace Core.SettingsPopup.Scripts
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