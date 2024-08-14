using System.Collections.Generic;
using System.Linq;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using VContainer;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewManager : IViewManager
    {
        private readonly Dictionary<string, IViewWrapper> _viewWrappers = new();
        private readonly Dictionary<string, IViewTypeManager> _viewEntities;
        private readonly Queue<IViewWrapper> _viewQueue = new();

        private bool _viewIsOpen;

        [Inject]
        public ViewManager(IViewAnimator backgroundAnimator)
        {
            _viewEntities = new Dictionary<string, IViewTypeManager>
            {
                { ViewType.Popup, new PopupViewTypeManager(_viewQueue, backgroundAnimator) },
                { ViewType.Window, new WindowViewTypeManager() },
                { ViewType.Service, new ServiceViewTypeManager() }
            };
        }

        public void Open(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            _viewIsOpen = _viewEntities[viewWrapper.ViewType].Open(viewWrapper, _viewIsOpen);
        }

        public void Close(string viewKey)
        {
            var viewWrapper = GetViewWrapper(viewKey);
            _viewIsOpen = _viewEntities[viewWrapper.ViewType].Close(viewWrapper, _viewIsOpen);
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
    }
}