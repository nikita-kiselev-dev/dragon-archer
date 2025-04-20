using Content.SettingsPopup.Scripts.Data;
using Content.SettingsPopup.Scripts.Model;
using Content.SettingsPopup.Scripts.Presenter;
using Content.SettingsPopup.Scripts.View;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Logger;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Content.SettingsPopup.Scripts
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.SettingsPopup)]
    public class SettingsPopup : ControlEntity, ISettingsPopup
    {
        [Inject] private readonly SettingsPopupData _data;
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        
        private readonly ILogManager _logger = new LogManager(nameof(SettingsPopup));
        
        private SettingsPopupView _view;
        private ISettingsPopupModel _model;
        private ISettingsPopupPresenter _presenter;

        public bool IsInited => _presenter.IsInited;

        protected override async UniTask Load()
        {
            await _assetLoader.LoadAsync<GameObject>(ViewInfo.SettingsPopup);
        }
        
        protected override async UniTask Init()
        {
            await CreateView();
            
            if (!IsLoadSucceed())
            {
                _logger.LogError($"{ViewInfo.SettingsPopup} load failed.");
                return;
            }
            
            CreateModel();
            CreatePresenter();
            _presenter.Init();
        }

        private async UniTask CreateView()
        {
            _view = await _viewFactory.CreateView<SettingsPopupView>(ViewInfo.SettingsPopup, ViewType.Popup);
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