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

        public IViewBuilder SetView(IMonoBehaviour view)
        {
            _viewWrapper.View = view;
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

        public IViewBuilder SetCustomOpenAnimation(IViewAnimator animator)
        {
            _viewWrapper.CustomOpenAnimation = animator.Show;
            return this;
        }

        public IViewBuilder SetCustomCloseAnimation(IViewAnimator animator)
        {
            _viewWrapper.CustomCloseAnimation = animator.Hide;
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
            
            _viewManager.RegisterView(_viewWrapper);

            if (_viewWrapper.View is IView view)
            {
                view.Init(_viewSignalManager);
            }
            
            return GetInteractor();
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