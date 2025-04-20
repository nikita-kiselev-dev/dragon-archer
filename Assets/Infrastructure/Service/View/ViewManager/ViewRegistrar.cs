using System;
using Infrastructure.Service.View.ViewManager.ViewAnimation;
using Infrastructure.Service.View.ViewSignalManager;

namespace Infrastructure.Service.View.ViewManager
{
    public class ViewRegistrar : IViewBuilder
    {
        private readonly IViewWrapper _viewWrapper = new ViewWrapper();
        private readonly IViewManager _viewManager;
        
        private  IViewSignalManager _viewSignalManager;

        public ViewRegistrar(IViewManager viewManager)
        {
            _viewManager = viewManager;
        }

        public IViewBuilder SetViewKey(string viewKey)
        {
            _viewWrapper.ViewKey = viewKey;
            return this;
        }

        public IViewBuilder SetViewType(string viewType)
        {
            _viewWrapper.ViewType = viewType;
            return this;
        }

        public IViewBuilder SetView(MonoView monoView)
        {
            _viewWrapper.View = monoView;
            return this;
        }

        public IViewBuilder SetViewSignalManager(IViewSignalManager viewSignalManager)
        {
            _viewSignalManager = viewSignalManager;
            return this;
        }

        public IViewBuilder EnableFromStart()
        {
            _viewWrapper.IsEnabledOnStart = true;
            return this;
        }

        public IViewBuilder SetAfterOpenAction(Action action)
        {
            _viewWrapper.AfterOpenAction = action;
            return this;
        }

        public IViewBuilder SetAfterCloseAction(Action action)
        {
            _viewWrapper.AfterCloseAction = action;
            return this;
        }

        public IViewBuilder SetCustomAnimator(IViewAnimator animator)
        {
            _viewWrapper.ViewAnimator = animator;
            return this;
        }

        public IViewInteractor RegisterAndInit()
        {
            if (string.IsNullOrEmpty(_viewWrapper.ViewKey))
                throw new InvalidOperationException("ViewBuilder: ViewKey is required!");
            
            if (string.IsNullOrEmpty(_viewWrapper.ViewType))
                throw new InvalidOperationException($"ViewBuilder: ViewType is required for {_viewWrapper.ViewKey}!");
            
            if (_viewWrapper.View == null)
                throw new InvalidOperationException($"ViewBuilder: View is required for {_viewWrapper.ViewKey}!");
            
            SetAnimator();
            _viewManager.RegisterView(_viewWrapper);
            _viewWrapper.View.Init(_viewSignalManager);
            
            return GetInteractor();
        }

        private void SetAnimator()
        {
            if (_viewWrapper.ViewAnimator != null)
            {
                return;
            }
            
            if (_viewWrapper.ViewType == ViewType.Popup)
            {
                _viewWrapper.ViewAnimator = new PopupAnimator(
                    _viewWrapper.View.gameObject.transform,
                    _viewWrapper.AfterOpenAction,
                    _viewWrapper.AfterCloseAction);
            }
            else
            {
                _viewWrapper.ViewAnimator = new WindowAnimator(
                    _viewWrapper.View.gameObject.transform,
                    _viewWrapper.AfterOpenAction,
                    _viewWrapper.AfterCloseAction);
            }
        }

        private IViewInteractor GetInteractor()
        {
            var openAction = new Action(() => _viewManager.Open(_viewWrapper.ViewKey));
            var closeAction = new Action(() => _viewManager.Close(_viewWrapper.ViewKey));
            var viewInteractor = new ViewInteractor(openAction, closeAction);
            return viewInteractor;
        }
    }
}