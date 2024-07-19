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
        [Inject] private readonly ServiceCanvas _serviceCanvas;
        
        public T CreateView<T>(string viewKey, string viewType = null)
        {
            var isServiceView = viewType == ViewType.Service;
            
            if (!isServiceView)
            {
                _mainCanvasController.PrepareCanvas(ViewInfo.MainCanvasKey);
            }

            var parent = isServiceView ? _serviceCanvas.transform : _mainCanvasController.GetParent(viewType);
            var operationHandler = _assetLoader.Instantiate<T>(viewKey, parent);
            
            return operationHandler.Result;
        }
    }
}