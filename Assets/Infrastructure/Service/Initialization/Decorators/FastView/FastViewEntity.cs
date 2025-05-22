using System.Reflection;

namespace Infrastructure.Service.Initialization.Decorators.FastView
{
    public class FastViewEntity
    {
        public FastView FastView { get; private set; }
        public FieldInfo FieldInfo { get; private set; }

        public FastViewEntity(FastView fastView, FieldInfo fieldInfo)
        {
            FastView = fastView;
            FieldInfo = fieldInfo;
        }
    }
}