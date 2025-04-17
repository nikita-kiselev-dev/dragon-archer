using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    [ControlEntityOrder(nameof(StartScope), (int)StartSceneInitOrder.CanvasManager)]
    [ControlEntityOrder(nameof(MetaScope), (int)MetaSceneInitOrder.CanvasManager)]
    public class CanvasManager : ControlEntity, ICanvasHandler
    {
        [Inject] private IAssetLoader _assetLoader;
        [Inject] private ISignalBus _signalBus;
        
        public IWindowCanvas WindowCanvas { get; private set; }
        public IPopupCanvas PopupCanvas { get; private set; }
        
        protected override async UniTask Load()
        {
            await _assetLoader.LoadAsync<GameObject>(ViewInfo.WindowCanvasKey);
            await _assetLoader.LoadAsync<GameObject>(ViewInfo.PopupCanvasKey);
        }
        
        protected override async UniTask Init()
        {
            await CreateView();
            WindowCanvas.Init();
            PopupCanvas.Init(PopupBackgroundClickAction);
        }

        private async UniTask CreateView()
        {
            WindowCanvas = await _assetLoader.InstantiateAsync<IWindowCanvas>(ViewInfo.WindowCanvasKey);
            PopupCanvas = await _assetLoader.InstantiateAsync<IPopupCanvas>(ViewInfo.PopupCanvasKey);
        }
        
        private void PopupBackgroundClickAction()
        {
            _signalBus.Trigger<OnPopupBackgroundClickSignal>();
        }
    }
}