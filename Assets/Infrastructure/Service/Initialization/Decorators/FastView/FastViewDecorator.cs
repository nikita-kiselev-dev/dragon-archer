using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infrastructure.Service.Asset;
using Infrastructure.Service.View.ViewFactory;
using Infrastructure.Service.View.ViewManager;
using VContainer;

namespace Infrastructure.Service.Initialization.Decorators.FastView
{
    public class FastViewDecorator : IControlEntityDecorator
    {
        [Inject] private readonly IViewFactory _viewFactory;
        [Inject] private readonly IViewManager _viewManager;
        [Inject] private readonly IAssetLoader _assetLoader;
        
        public ControlEntity Decorate(ControlEntity controlEntity)
        {
            var isDecoratable = controlEntity
                .GetType()
                .GetCustomAttributes(typeof(FastViewDecoratable), inherit: false)
                .Any();

            return isDecoratable ? 
                Create(controlEntity) : 
                controlEntity;
        }
        
        private FieldInfo[] GetFieldsWithFastViewAttribute(Type type)
        {
            return type
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(fieldInfo => fieldInfo.GetCustomAttributes(typeof(FastView), inherit: false).Any())
                .ToArray();
        }

        private ControlEntity Create(ControlEntity controlEntity)
        {
            var fastViewFields = GetFieldsWithFastViewAttribute(controlEntity.GetType());
            var fastViewEntities = new HashSet<FastViewEntity>();

            foreach (var field in fastViewFields)
            {
                var attribute = (FastView)field.GetCustomAttributes(typeof(FastView), false).First();
                fastViewEntities.Add(new FastViewEntity(attribute, field));
            }
                
            return new FastViewControlEntity(
                controlEntity, 
                fastViewEntities, 
                _viewFactory, 
                _viewManager , 
                _assetLoader);
        }
    }
}