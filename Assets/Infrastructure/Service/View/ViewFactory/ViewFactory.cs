using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Scopes;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Infrastructure.Service.View.ViewFactory
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.ViewFactory)]
    [ControlEntityOrder(nameof(CoreScope), (int)CoreSceneInitOrder.ViewFactory)]
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.ViewFactory)]
    public class ViewFactory : ControlEntity, IViewFactory, ICanvasHandler, IDisposable
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ISignalBus _signalBus;

        private IWindowCanvas _windowCanvas;
        private IPopupCanvas _popupCanvas;

        public Image PopupCanvasBackground => _popupCanvas.BackgroundImage;
        
        public async UniTask<T> CreateView<T>(string viewKey, string viewType = null)
        {
            var parentTransform = GetParentTransform(viewType);
            var operationHandler = await _assetLoader.InstantiateAsync<T>(viewKey, parentTransform);
            
            return operationHandler;
        }
        
        protected override async UniTask Load()
        {
            _windowCanvas = await _assetLoader.InstantiateAsync<IWindowCanvas>(ViewInfo.WindowCanvasKey);
            _popupCanvas = await _assetLoader.InstantiateAsync<IPopupCanvas>(ViewInfo.PopupCanvasKey);
        }
        
        protected override UniTask Init()
        {
            _windowCanvas.Init();
            _popupCanvas.Init(PopupBackgroundClickAction);
            
            return UniTask.CompletedTask;
        }

        private Transform GetParentTransform(string viewType)
        {
            return viewType switch
            {
                ViewType.Window => _windowCanvas.ViewParentTransform,
                ViewType.Popup => _popupCanvas.ViewParentTransform,
                _ => ServiceCanvas.Instance.transform
            };
        }

        private void PopupBackgroundClickAction()
        {
            _signalBus.Trigger<OnPopupBackgroundClickSignal>();
        }

        void IDisposable.Dispose()
        {
            _windowCanvas = null;
            _popupCanvas = null;
        }
    }
}