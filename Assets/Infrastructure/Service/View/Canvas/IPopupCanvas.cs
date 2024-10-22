using System;
using UnityEngine.UI;

namespace Infrastructure.Service.View.Canvas
{
    public interface IPopupCanvas : ICanvas
    {
        Image BackgroundImage { get; }
        void Init(Action onBackgroundClicked);
    }
}