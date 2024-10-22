using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Scopes;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewFactory
{
    [ControlEntityOrder(nameof(BootstrapScope), -1)]
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.ViewFactory)]
    [ControlEntityOrder(nameof(CoreScope), (int)CoreSceneInitOrder.ViewFactory)]
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.ViewFactory)]
    public class ViewFactory : ControlEntity, IViewFactory
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly IMainCanvasController _mainCanvasController;
        [Inject] private readonly IBackgroundViewActionHandler _backgroundViewActionHandler;

        private IWindowCanvas _windowCanvas;
        private IPopupCanvas _popupCanvas;

        protected override async UniTask Load()
        {
            _windowCanvas = await _assetLoader.InstantiateAsync<IWindowCanvas>(ViewInfo.WindowCanvasKey);
            _popupCanvas = await _assetLoader.InstantiateAsync<IPopupCanvas>(ViewInfo.PopupCanvasKey);
        }
        
        protected override UniTask Init()
        {
            _windowCanvas.Init();
            _popupCanvas.Init(_backgroundViewActionHandler.BackgroundViewAction);
            
            return UniTask.CompletedTask;
        }
        
        public async UniTask<T> CreateView<T>(string viewKey, string viewType = null)
        {
            var parent = GetParent(viewType);
            var operationHandler = await _assetLoader.InstantiateAsync<T>(viewKey, parent);
            
            return operationHandler;
        }

        private Transform GetParent(string viewType)
        {
            switch (viewType)
            {
                case ViewType.Window:
                    return _windowCanvas.ViewParentTransform;
                case ViewType.Popup:
                    return _popupCanvas.ViewParentTransform;
                default:
                    return ServiceCanvas.Instance.transform;
            }
        }
    }
}