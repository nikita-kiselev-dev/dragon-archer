namespace Infrastructure.Service.View.ViewManager
{
    public interface IViewTypeManager
    {
        public bool Open(IViewWrapper viewWrapper, bool viewIsOpen);
        public bool Close(IViewWrapper viewWrapper, bool viewIsOpen);
    }
}