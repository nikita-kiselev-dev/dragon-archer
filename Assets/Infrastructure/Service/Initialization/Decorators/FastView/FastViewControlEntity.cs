using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using Infrastructure.Service.Asset;
using Infrastructure.Service.Logger;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using UnityEngine;

namespace Infrastructure.Service.Initialization.Decorators.FastView
{
    public class FastViewControlEntity : ControlEntity
    {
        private readonly ControlEntity _baseControlEntity;
        private readonly HashSet<FastViewEntity> _fastViews;
        private readonly IViewFactory _viewFactory;
        private readonly IViewManager _viewManager;
        private readonly IAssetLoader _assetLoader;
        private readonly LogManager _logger = new(nameof(FastViewControlEntity));
        private readonly Dictionary<string, bool> _viewCreationStatuses = new();

        public FastViewControlEntity(
            ControlEntity controlEntity, 
            HashSet<FastViewEntity> fastViews,
            IViewFactory viewFactory,
            IViewManager viewManager,
            IAssetLoader assetLoader)
        {
            _baseControlEntity = controlEntity;
            _fastViews = fastViews;
            _viewFactory = viewFactory;
            _viewManager = viewManager;
            _assetLoader = assetLoader;
        }
        
        protected override async UniTask Load()
        {
            await LoadViews();
            await _baseControlEntity.LoadPhase();
        }

        protected override async UniTask Init()
        {
            await CreateViews();
            var isAnyCreationFailed = _viewCreationStatuses.Any(status => !status.Value);
            _viewCreationStatuses.Clear();

            if (isAnyCreationFailed)
            {
                _logger.LogError($"{_baseControlEntity.GetType()} - service can't create views, something went wrong.");
                return;
            }
            
            await _baseControlEntity.InitPhase();
        }

        private async UniTask LoadViews()
        {
            var fastViewsToLoad = Enumerable
                .Select(_fastViews, fastView => UniTask.Defer(() => LoadView(fastView)))
                .ToList();

            await UniTask.WhenAll(fastViewsToLoad);
        }

        private async UniTask LoadView(FastViewEntity fastView)
        {
            await _assetLoader.LoadAsync<GameObject>(fastView.FastView.ViewKey);
        }
        
        private async UniTask CreateViews()
        {
            var fastViewsToCreate = Enumerable
                .Select(_fastViews, fastView => UniTask.Defer(() => CreateViewByType(fastView)))
                .ToList();

            await UniTask.WhenAll(fastViewsToCreate);
        }
        
        private async UniTask CreateViewByType(FastViewEntity fastView)
        {
            var createViewMethod = typeof(FastViewControlEntity)
                .GetMethod(nameof(CreateView), BindingFlags.NonPublic | BindingFlags.Instance)
                ?.MakeGenericMethod(fastView.FieldInfo.FieldType);

            if (createViewMethod != null)
            {
                var creationTask = (UniTask<MonoView>)createViewMethod.Invoke(this, new object[] { fastView });
                await creationTask;
            }
        }

        private async UniTask<MonoView> CreateView<T>(FastViewEntity fastView) where T : MonoView
        {
            var view = await _viewFactory.CreateView<T>(fastView.FastView.ViewKey, fastView.FastView.ViewType);
            _viewCreationStatuses.Add(fastView.FastView.ViewKey, view);
            RegisterAndInitView(fastView.FastView, view);
            fastView.FieldInfo.SetValue(_baseControlEntity, view);
            return view;
        }
        
        private void RegisterAndInitView(FastView fastView, MonoView view)
        {
            var viewInteractor = new ViewRegistrar(_viewManager)
                .SetViewKey(fastView.ViewKey)
                .SetViewType(fastView.ViewType)
                .SetView(view)
                .EnableFromStart(fastView.ViewType != ViewType.Popup)
                .RegisterAndInit();

            if (view is IViewInteractorContainer viewInteractorContainer)
            {
                viewInteractorContainer.SetViewInteractor(viewInteractor);
            }
        }
    }
}