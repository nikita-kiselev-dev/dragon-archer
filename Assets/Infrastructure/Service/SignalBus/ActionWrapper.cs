using System;

namespace Infrastructure.Service.SignalBus
{
    public class ActionWrapper : IActionWrapper
    {
        private Action _action;
        private object _invoker;

        public ActionWrapper(object invoker, Action action)
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
            
            _action?.Invoke();

            return true;
        }
    }
}