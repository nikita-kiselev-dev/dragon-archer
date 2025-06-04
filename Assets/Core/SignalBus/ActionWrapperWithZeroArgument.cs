using System;

namespace Core.SignalBus
{
    public class ActionWrapperWithZeroArgument : IActionWrapper
    {
        private Delegate _action;
        private object _invoker;

        public ActionWrapperWithZeroArgument(object invoker, Delegate action)
        {
            _action = action;
            _invoker = invoker;
        }

        public bool Invoke()
        {
            if (_invoker == null)
            {
                return false;
            }
            
            _action?.DynamicInvoke();

            return true;
        }
    }
}