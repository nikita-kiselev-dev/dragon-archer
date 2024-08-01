using System;
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

        private MainCanvas _canvas;
        
        public void TryCreateCanvas(Action onBackgroundClicked)
        {
            if (_canvas)
            {
                return;
            }
            
            _canvas = _assetLoader.Instantiate<MainCanvas>(ViewInfo.MainCanvasKey).Result;
            _canvas.Init(onBackgroundClicked);
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