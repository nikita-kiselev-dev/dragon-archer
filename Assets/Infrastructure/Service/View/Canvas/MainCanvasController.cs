using Infrastructure.Service.Asset;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace Infrastructure.Service.View.Canvas
{
    public class MainCanvasController : IMainCanvasController
    {
        [Inject] private readonly IAssetLoader _assetLoader;

        private MainCanvas _canvas;
        
        public void PrepareCanvas(string canvasKey)
        {
            if (_canvas)
            {
                return;
            }
            
            _canvas = _assetLoader.Instantiate<MainCanvas>(canvasKey).Result;
            _canvas.Init();
        }

        public Image GetPopupBackground()
        {
            return _canvas.PopupBackground;
        }
        
        public Transform GetParent(string viewType)
        {
            return _canvas.GetParent(viewType);
        }
    }
}