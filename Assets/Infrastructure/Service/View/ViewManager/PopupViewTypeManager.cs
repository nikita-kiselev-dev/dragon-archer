using System.Collections.Generic;
using System.Linq;
using Infrastructure.Service.View.ViewManager.ViewAnimation;

namespace Infrastructure.Service.View.ViewManager
{
    public class PopupViewTypeManager : IViewTypeManager
    {
        private readonly Queue<IViewWrapper> _viewQueue;
        private readonly IViewAnimator _backgroundAnimator;

        public PopupViewTypeManager(Queue<IViewWrapper> viewQueue, IViewAnimator backgroundAnimator)
        {
            _viewQueue = viewQueue;
            _backgroundAnimator = backgroundAnimator;
        }
        
        public bool Open(IViewWrapper viewWrapper, bool viewIsOpen)
        {
            _viewQueue.Enqueue(viewWrapper);
            
            if (viewIsOpen)
            {
                return false;
            }
            
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

                _backgroundAnimator.Show();

                new PopupAnimator(monoBehaviour.transform).Show();
            }

            return true;
        }

        public bool Close(IViewWrapper viewWrapper)
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

                _backgroundAnimator.Hide();
                
                new PopupAnimator(monoBehaviour.transform).Hide();
            }
            
            if (_viewQueue.LastOrDefault() != viewWrapper)
            {
                return false;
            }
            
            _viewQueue.Dequeue();
            
            return false;
        }
    }
}