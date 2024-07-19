using System;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewInteractor : IViewInteractor
    {
        private readonly Action _open;
        private readonly Action _close;
        
        public ViewInteractor(Action open, Action close)
        {
            _open = open;
            _close = close;
        }

        public void Open()
        {
            _open?.Invoke();
        }

        public void Close()
        {
            _close?.Invoke();
        }
    }
}