using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Infrastructure.Service.View.ViewFactory
{
    public class ViewFactory : IViewFactory
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly IMainCanvasController _mainCanvasController;
        [Inject] private readonly IBackgroundViewActionHandler _backgroundViewActionHandler;
        
        public async UniTask<T> CreateView<T>(string viewKey, string viewType = null)
        {
            var isServiceView = viewType == ViewType.Service;
            
            var parent = isServiceView ? 
                ServiceCanvas.Instance.transform : 
                await _mainCanvasController.GetParent(viewType, _backgroundViewActionHandler.BackgroundViewAction);
            
            var operationHandler = await _assetLoader.InstantiateAsync<T>(viewKey, parent);
            
            return operationHandler;
        }
    }
}