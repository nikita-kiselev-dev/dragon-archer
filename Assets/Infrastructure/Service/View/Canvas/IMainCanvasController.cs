using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public interface IMainCanvasController : ICanvasController
    {
        public UniTask<Transform> GetParent(string viewType, Action onBackgroundClickAction);
        public Image GetPopupBackground();
    }
}