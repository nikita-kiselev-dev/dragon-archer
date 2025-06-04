using Core.Asset;
using Core.View.Canvas.Scripts;
using Core.View.ViewManager;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Core.View.ViewFactory
{
    public class ViewFactory : IViewFactory
    {
        [Inject] private readonly IAssetLoader _assetLoader;
        [Inject] private readonly ICanvasHandler _canvasHandler;
        
        public async UniTask<T> CreateView<T>(string viewKey, string viewType = null)
        {
            var parentTransform = GetParentTransform(viewType);
            var operationHandler = await _assetLoader.InstantiateAsync<T>(viewKey, parentTransform);
            if (operationHandler is not MonoView view) return operationHandler; 
            var startViewStatus = GetStartViewStatus(viewType);
            view.gameObject.SetActive(startViewStatus);

            return operationHandler;
        }

        private Transform GetParentTransform(string viewType)
        {
            if (viewType == ViewType.Window) return _canvasHandler.WindowCanvas.ViewParentTransform;
            if (viewType == ViewType.Popup) return _canvasHandler.PopupCanvas.ViewParentTransform;
            return ServiceCanvas.Instance.transform;
        }

        private bool GetStartViewStatus(string viewType)
        { 
            return viewType == ViewType.Window;
        }
    }
}