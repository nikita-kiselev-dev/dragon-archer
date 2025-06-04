namespace Core.View.ViewManager
{
    public interface IViewInteractorContainer
    {
        IViewInteractor ViewInteractor { get; }
        void SetViewInteractor(IViewInteractor viewInteractor);
    }
}