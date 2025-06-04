using Cysharp.Threading.Tasks;

namespace Core.View.ViewFactory
{
    public interface IViewFactory : IFactory
    {
        public UniTask<T> CreateView<T>(string viewKey, string viewType);
    }
}