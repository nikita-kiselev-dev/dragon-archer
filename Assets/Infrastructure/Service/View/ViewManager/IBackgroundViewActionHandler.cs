using System;

namespace Infrastructure.Service.View.ViewManager
{
    public interface IBackgroundViewActionHandler
    {
        public Action BackgroundViewAction { get; }
    }
}