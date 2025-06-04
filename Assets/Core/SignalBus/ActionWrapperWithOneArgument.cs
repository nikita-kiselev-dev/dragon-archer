using UnityEngine.Events;

namespace Core.SignalBus
{
    public class ActionWrapperWithOneArgument<U> : IActionWrapper
    {
        private UnityAction<U> _action;
        private object _invoker;

        public ActionWrapperWithOneArgument(object invoker, UnityAction<U> action)
        {
            _action = action;
            _invoker = invoker;
        }

        public bool Invoke(U arg)
        {
            if (_invoker == null)
            {
                return false;
            }
            
            _action?.DynamicInvoke(arg);

            return true;
        }
    }
}