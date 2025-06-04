namespace Core.View.ViewManager
{
    public abstract class MonoView : UnityEngine.MonoBehaviour, IViewInteractorContainer
    {
        public IViewInteractor ViewInteractor { get; private set; }

        void IViewInteractorContainer.SetViewInteractor(IViewInteractor viewInteractor)
        {
            ViewInteractor = viewInteractor;
        }
    }
}