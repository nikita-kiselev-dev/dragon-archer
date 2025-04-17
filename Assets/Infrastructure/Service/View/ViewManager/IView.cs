using Infrastructure.Service.View.ViewSignalManager;

namespace Infrastructure.Service.View.ViewManager

{
    public abstract class IView : View
    {
        public abstract void Init(IViewSignalManager viewSignalManager);
    }
}