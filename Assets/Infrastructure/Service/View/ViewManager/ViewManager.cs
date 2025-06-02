using System;
using System.Collections.Generic;
using System.Linq;
using Content.LoadingCurtain.Scripts;
using Infrastructure.Service.Initialization;
using Infrastructure.Service.Initialization.InitOrder;
using Infrastructure.Service.Initialization.Scopes;
using Infrastructure.Service.Logger;
using Infrastructure.Service.SignalBus;
using Infrastructure.Service.View.Canvas;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using VContainer;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewManager : IViewManager, IDisposable
    {
        private readonly ISignalBus _signalBus;
        private readonly Dictionary<string, IViewWrapper> _viewWrappers = new();
        private readonly Dictionary<string, IViewTypeManager> _viewTypeManagers;
        private readonly Queue<IViewWrapper> _viewQueue = new();
        private readonly BackgroundAnimator _backgroundAnimator = new();
        private readonly ILogManager _logger = new LogManager(nameof(ViewManager));
        
        private HashSet<string> _delayedViewKeys = new();
        private bool _isActive;
        private bool _viewIsOpen;

        [Inject]
        private ViewManager(ISignalBus signalBus, ICanvasHandler canvasHandler)
        {
            _signalBus = signalBus;
            _backgroundAnimator.SetCanvasManager(canvasHandler);
            
            _viewTypeManagers = new Dictionary<string, IViewTypeManager>
            {
                { ViewType.Popup, new PopupViewTypeManager(_viewQueue, _backgroundAnimator) },
                { ViewType.Window, new WindowViewTypeManager() },
                { ViewType.Service, new ServiceViewTypeManager() }
            };
            
            Subscribe();
        }

        public void Open(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            
            if (!_isActive && viewWrapper.ViewType == ViewType.Popup)
            {
                _delayedViewKeys.Add(viewKey);
                return;
            }

            if (!viewWrapper?.View)
            {
                _logger.LogError($"{viewKey} - view wrapper or view is null.");
                return;
            }
            
            var isSucceed = _viewTypeManagers[viewWrapper.ViewType].Open(viewWrapper, _viewIsOpen);
            if (viewWrapper.ViewType == ViewType.Service) return;
            _viewIsOpen = isSucceed;
        }

        public void Close(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            
            if (viewWrapper?.View == null)
            {
                _logger.LogError($"{viewKey} - view wrapper or view is null.");
                return;
            }

            _viewTypeManagers[viewWrapper.ViewType].Close(viewWrapper);
            if (viewWrapper.ViewType == ViewType.Service) return;
            _viewIsOpen = false;
            OpenNext();
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
            if (lastViewExists) Close(lastView?.ViewKey);
        }

        public void RegisterView(IViewWrapper viewWrapper)
        {
            _viewWrappers.Add(viewWrapper.ViewKey, viewWrapper);
            ConfigureAfterRegistration(viewWrapper);
        }
        
        void IDisposable.Dispose()
        {
            _isActive = false;
            _viewWrappers.Clear();
            _viewQueue.Clear();
            Unsubscribe();
        }
        
        private void ExecuteDelayedViews()
        {
            _isActive = true;
            
            foreach (var viewKey in _delayedViewKeys)
            {
                Open(viewKey);
            }

            _delayedViewKeys = null;
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
            view.gameObject.SetActive(viewWrapper.IsEnabledOnStart);
        }

        private void Subscribe()
        {
            _signalBus.Subscribe<OnPopupBackgroundClickSignal>(this, CloseLast);
            _signalBus.Subscribe<LoadingCurtainHiddenSignal>(this, ExecuteDelayedViews);
        }

        private void Unsubscribe()
        {
            _signalBus.Unsubscribe<OnPopupBackgroundClickSignal>(this);
            _signalBus.Unsubscribe<LoadingCurtainHiddenSignal>(this);
        }
    }
}