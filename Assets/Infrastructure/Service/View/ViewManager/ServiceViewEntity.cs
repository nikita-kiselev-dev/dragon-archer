using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;

namespace Infrastructure.Service.View.ViewManager
{
    public class ServiceViewEntity : IViewEntity
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
                if (viewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return false;
                }

                new WindowAnimator(viewMonoBehaviour.transform).Show();
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
                if (viewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return false;
                }
                
                new WindowAnimator(viewMonoBehaviour.transform).Hide();
            }
            
            return false;
        }
    }
}