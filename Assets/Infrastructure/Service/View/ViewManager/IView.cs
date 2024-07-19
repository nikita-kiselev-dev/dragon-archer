using Infrastructure.Service.View.ViewSignalManager;

namespace Infrastructure.Service.View.ViewManager

{
    public interface IView : IMonoBehaviour
    {
        public void Init(IViewSignalManager viewSignalManager);
    }
}