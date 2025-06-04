using System.Reflection;

namespace Core.Initialization.Scripts.Decorators.FastView
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