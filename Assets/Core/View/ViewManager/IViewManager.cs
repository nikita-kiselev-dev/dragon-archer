namespace Core.View.ViewManager
{
    public interface IViewManager
    {
        public void Open(string viewKey);
        public void Close(string viewKey);
        public void CloseAll();
        public void CloseLast();
        public void RegisterView(IViewWrapper viewWrapper);
    }
}