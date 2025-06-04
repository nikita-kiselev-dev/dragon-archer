using System.Collections.Generic;
using System.Linq;
using Core.View.ViewManager.ViewAnimation;

namespace Core.View.ViewManager
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

            _backgroundAnimator.Show();
            viewWrapper.ViewAnimator.Show();
            return true;
        }

        public bool Close(IViewWrapper viewWrapper)
        {
            _backgroundAnimator.Hide();
            viewWrapper.ViewAnimator.Hide();

            if (_viewQueue.LastOrDefault() != viewWrapper)
            {
                return false;
            }
            
            _viewQueue.Dequeue();
            return true;
        }
    }
}