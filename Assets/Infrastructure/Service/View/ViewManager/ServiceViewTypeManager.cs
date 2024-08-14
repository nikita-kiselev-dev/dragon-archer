using Infrastructure.Service.View.ViewManager.ViewAnimation;

namespace Infrastructure.Service.View.ViewManager
{
    public class ServiceViewTypeManager : IViewTypeManager
    {
        public bool Open(IViewWrapper viewWrapper, bool viewIsOpen)
        {
            var customAnimation = viewWrapper.CustomOpenAnimation;

            if (customAnimation != null)
            {
                customAnimation();
            }
            else
            {
                var monoBehaviour = viewWrapper.View.MonoBehaviour;
                
                if (!monoBehaviour)
                {
                    return false;
                }

                new WindowAnimator(monoBehaviour.transform).Show();
            }

            return true;
        }

        public bool Close(IViewWrapper viewWrapper, bool viewIsOpen)
        {
            var customAnimation = viewWrapper.CustomCloseAnimation;

            if (customAnimation != null)
            {
                customAnimation();
            }
            else
            {
                var monoBehaviour = viewWrapper.View.MonoBehaviour;
                
                if (!monoBehaviour)
                {
                    return false;
                }
                
                new WindowAnimator(monoBehaviour.transform).Hide();
            }
            
            return false;
        }
    }
}