using System.Collections.Generic;
using System.Linq;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;

namespace Infrastructure.Service.View.ViewManager
{
    public class PopupViewEntity : IViewEntity
    {
        private readonly Queue<IViewWrapper> _viewQueue;
        private readonly IViewAnimator _backgroundAnimator;

        public PopupViewEntity(Queue<IViewWrapper> viewQueue, IViewAnimator backgroundAnimator)
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
                if (viewWrapper is not { View: MonoBehaviour viewMonoBehaviour })
                {
                    return false;
                }

                _backgroundAnimator.Show();

                new PopupAnimator(viewMonoBehaviour.transform).Show();
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

                _backgroundAnimator.Hide();
                
                new PopupAnimator(viewMonoBehaviour.transform).Hide();
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