using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Infrastructure.Service.SignalBus
{
    public class EventSignalBus : ISignalBus
    {
        private Dictionary<Type, Dictionary<object, IActionWrapper>> _events = new();

        public void Subscribe<T>(object listener, Action action)
        {
            var type = typeof(T);
            
            if (AddSubscriber<T>(listener))
            {
                _events[type].Add(listener, new ActionWrapper(listener, action));
            }
        }

        public void Unsubscribe<T>(object listener)
        {
            var type = typeof(T);

            if (_events.ContainsKey(type))
            {
                _events[type].Remove(listener);
            }
        }

        public void Trigger<T>()
        {
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            foreach (var wrapper in 
                     events.Select(actionWrapper => actionWrapper.Value))
            {
                wrapper?.Invoke();
            }
        }

        private bool AddSubscriber<T>(object listener)
        {
            var type = typeof(T);
            
            if (!_events.ContainsKey(type))
            {
                _events.Add(type, new Dictionary<object, IActionWrapper>());
            }

            if (!_events[type].ContainsKey(listener))
            {
                return true;
            }
            
            Debug.LogError($"{GetType().Name}: you try to subscribe on {type} twice with object: " +
                           $"{listener} type: {listener.GetType()}");
            
            return false;
        }
    }
}