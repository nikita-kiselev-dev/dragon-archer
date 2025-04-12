using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewFactory
{
    public class ViewFactory : IViewFactory
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ICanvasHandler _canvasHandler;
        
        public async UniTask<T> CreateView<T>(string viewKey, string viewType = null)
        {
            var parentTransform = GetParentTransform(viewType);
            var operationHandler = await _assetLoader.InstantiateAsync<T>(viewKey, parentTransform);
            if (operationHandler is not IMonoBehaviour monoBehaviour) return operationHandler; 
            var startViewStatus = GetStartViewStatus(viewType);
            monoBehaviour.SetActive(startViewStatus);

            return operationHandler;
        }

        private Transform GetParentTransform(string viewType)
        {
            return viewType switch
            {
                ViewType.Window => _canvasHandler.WindowCanvas.ViewParentTransform,
                ViewType.Popup => _canvasHandler.PopupCanvas.ViewParentTransform,
                _ => ServiceCanvas.Instance.transform
            };
        }

        private bool GetStartViewStatus(string viewType)
        { 
            return viewType switch
            {
                ViewType.Window => true,
                _ => false
            };
        }
    }
}