using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public interface IMainCanvasController : ICanvasController
    {
        public Image GetPopupBackground();
        public Transform GetParent(string viewType);
    }
}