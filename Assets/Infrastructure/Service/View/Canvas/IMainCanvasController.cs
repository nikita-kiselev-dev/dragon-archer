using System;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public interface IMainCanvasController : ICanvasController
    {
        public void TryCreateCanvas(Action onBackgroundClicked);
        public Image GetPopupBackground();
        public Transform GetParent(string viewType);
    }
}