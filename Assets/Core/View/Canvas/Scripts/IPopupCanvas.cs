using System;
using UnityEngine.UI;

namespace Core.View.Canvas.Scripts
{
    public interface IPopupCanvas : ICanvas
    {
        Image BackgroundImage { get; }
        void Init(Action onBackgroundClicked);
    }
}