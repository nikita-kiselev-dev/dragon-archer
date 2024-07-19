namespace Infrastructure.Service.View.ViewFactory
{
    public interface IViewFactory : IFactory
    {
        public T CreateView<T>(string viewKey, string viewType);
    }
}