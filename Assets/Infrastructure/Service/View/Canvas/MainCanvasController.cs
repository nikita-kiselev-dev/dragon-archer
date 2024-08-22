using System;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    public class MainCanvasController : IMainCanvasController
    {
        [Inject] private readonly IAssetLoader _assetLoader;

        private UniTaskCompletionSource<Transform> _canvasCreationSource;
        private MainCanvas _canvas;
        
        public async UniTask<Transform> GetParent(string viewType, Action onBackgroundClickAction)
        {
            if (_canvas)
            {
                return GetViewParent(viewType);
            }

            if (_canvasCreationSource != null)
            {
                await _canvasCreationSource.Task;
            }
            else
            {
                _canvasCreationSource = new UniTaskCompletionSource<Transform>();
                await CreateCanvasAsync(onBackgroundClickAction);
                
                _canvasCreationSource.TrySetResult(GetViewParent(viewType));
                _canvasCreationSource = null;
            }

            return GetViewParent(viewType);
        }

        public Image GetPopupBackground()
        {
            return _canvas.PopupBackground;
        }
        
        private Transform GetViewParent(string viewType)
        {
            return _canvas.GetParent(viewType);
        }

        private async UniTask CreateCanvasAsync(Action onBackgroundClickAction)
        {
            _canvas = await _assetLoader.InstantiateAsync<MainCanvas>(ViewInfo.MainCanvasKey);
            _canvas.Init(onBackgroundClickAction);
        }
    }
}