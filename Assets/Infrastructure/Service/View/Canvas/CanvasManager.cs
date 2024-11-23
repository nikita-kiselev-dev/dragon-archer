using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.CanvasManager)]
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.CanvasManager)]
    public class CanvasManager : ControlEntity, ICanvasHandler, IDisposable
    {
        [Inject] private IAssetLoader _assetLoader;
        [Inject] private ISignalBus _signalBus;
        
        public IWindowCanvas WindowCanvas { get; private set; }
        public IPopupCanvas PopupCanvas { get; private set; }
        
        protected override async UniTask Load()
        {
            WindowCanvas = await _assetLoader.InstantiateAsync<IWindowCanvas>(ViewInfo.WindowCanvasKey);
            PopupCanvas = await _assetLoader.InstantiateAsync<IPopupCanvas>(ViewInfo.PopupCanvasKey);
        }
        
        protected override UniTask Init()
        {
            WindowCanvas.Init();
            PopupCanvas.Init(PopupBackgroundClickAction);
            
            return UniTask.CompletedTask;
        }
        
        void IDisposable.Dispose()
        {
            WindowCanvas = null;
            PopupCanvas = null;
        }
        
        private void PopupBackgroundClickAction()
        {
            _signalBus.Trigger<OnPopupBackgroundClickSignal>();
        }
    }
}