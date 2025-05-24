using UnityEngine;

namespace Infrastructure.Service.View.ViewManager
{
    public abstract class MonoView : MonoBehaviour, IViewInteractorContainer
    {
        public IViewInteractor ViewInteractor { get; private set; }

        void IViewInteractorContainer.SetViewInteractor(IViewInteractor viewInteractor)
        {
            ViewInteractor = viewInteractor;
        }
    }
}