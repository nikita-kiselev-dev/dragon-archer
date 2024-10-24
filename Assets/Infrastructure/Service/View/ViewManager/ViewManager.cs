using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using UnityEngine;
using VContainer;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewManager : IViewManager, IDisposable
    {
        private readonly Dictionary<string, IViewWrapper> _viewWrappers = new();
        private readonly Dictionary<string, IViewTypeManager> _viewTypeManagers;
        private readonly Queue<IViewWrapper> _viewQueue = new();

        private bool _viewIsOpen;

        [Inject]
        private ViewManager(ISignalBus signalBus, IViewAnimator backgroundAnimator)
        {
            signalBus.Subscribe<OnPopupBackgroundClickSignal>(this, CloseLast);
            
            _viewTypeManagers = new Dictionary<string, IViewTypeManager>
            {
                { ViewType.Popup, new PopupViewTypeManager(_viewQueue, backgroundAnimator) },
                { ViewType.Window, new WindowViewTypeManager() },
                { ViewType.Service, new ServiceViewTypeManager() }
            };
        }

        public void Open(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);

            if (viewWrapper?.View == null)
            {
                Debug.LogError($"{GetType().Name}: {viewKey} - view wrapper or view is null!");
                return;
            }
            
            var viewIsOpen = _viewTypeManagers[viewWrapper.ViewType].Open(viewWrapper, _viewIsOpen);

            if (viewWrapper.ViewType == ViewType.Service)
            {
                return;
            }
            
            _viewIsOpen = viewIsOpen;
        }

        public void Close(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            
            if (viewWrapper?.View == null)
            {
                Debug.LogError($"{GetType().Name}: {viewKey} - view wrapper or view is null!");
                return;
            }

            var viewIsOpen = _viewTypeManagers[viewWrapper.ViewType].Close(viewWrapper);

            if (viewWrapper.ViewType == ViewType.Service)
            {
                return;
            }
            
            OpenNext();
            _viewIsOpen = viewIsOpen;
        }

        public void CloseAll()
        {
            foreach (var view in _viewWrappers)
            {
                Close(view.Value.ViewKey);
            }
        }

        public void CloseLast()
        {
            var lastView = _viewQueue.LastOrDefault();
            var lastViewExists = _viewQueue.LastOrDefault() != null;

            if (lastViewExists)
            {
                Close(lastView?.ViewKey);
            }
        }

        public void RegisterView(IViewWrapper viewWrapper)
        {
            _viewWrappers.Add(viewWrapper.ViewKey, viewWrapper);
            ConfigureAfterRegistration(viewWrapper);
        }

        private void OpenNext()
        {
            var lastView = _viewQueue.LastOrDefault();
            var lastViewExists = _viewQueue.LastOrDefault() != null;

            if (lastViewExists)
            {
                Open(lastView?.ViewKey);
            }
        }

        private IViewWrapper GetViewWrapper(string viewKey)
        {
            _viewWrappers.TryGetValue(viewKey, out var viewWrapper);
            return viewWrapper;
        }

        private void ConfigureAfterRegistration(IViewWrapper viewWrapper)
        {
            var view = viewWrapper.View;
            view.SetActive(viewWrapper.IsEnabledOnStart);
        }

        void IDisposable.Dispose()
        {
            _viewWrappers.Clear();
            _viewQueue.Clear();
        }
    }
}