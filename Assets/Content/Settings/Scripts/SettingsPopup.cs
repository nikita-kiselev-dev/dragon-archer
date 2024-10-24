using Content.Settings.Scripts.Data;
using Content.Settings.Scripts.Model;
using Content.Settings.Scripts.Presenter;
using Content.Settings.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Logger;
using Infrastructure.Service.Scopes;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Content.Settings.Scripts
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.SettingsPopup)]
    public class SettingsPopup : ControlEntity, ISettingsPopup
    {
        [Inject] private readonly SettingsPopupData _data;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        
        private readonly ILogManager _logger = new LogManager(nameof(SettingsPopup));
        
        private ISettingsPopupView _view;
        private ISettingsPopupModel _model;
        private ISettingsPopupPresenter _presenter;

        public bool IsInited => _presenter.IsInited;

        protected override async UniTask Load()
        {
            _view = await _viewFactory.CreateView<ISettingsPopupView>(ViewInfo.SettingsPopup, ViewType.Popup);
        }
        
        protected override UniTask Init()
        {
            if (!IsLoadSucceed())
            {
                _logger.LogError($"{ViewInfo.SettingsPopup} load failed.");
                return UniTask.CompletedTask;
            }
            
            CreateModel();
            CreatePresenter();
            _presenter.Init();

            return UniTask.CompletedTask;
        }
        
        private bool IsLoadSucceed()
        {
            var result = _view is not null;
            return result;
        }

        private void CreateModel()
        {
            _model = new SettingsPopupModel(_data);
        }

        private void CreatePresenter()
        {
            _presenter = new SettingsPopupPresenter(_model, _view, _viewManager);
        }
    }
}