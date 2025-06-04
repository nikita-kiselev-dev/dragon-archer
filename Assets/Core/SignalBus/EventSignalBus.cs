using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Core.SignalBus
{
    public class EventSignalBus : ISignalBus
    {
        private Dictionary<Type, Dictionary<object, IActionWrapper>> _events;

        public void Subscribe<T>(object listener, UnityAction action)
        {
            var type = typeof(T);
            
            if (AddSubscriber<T>(listener))
            {
                _events[type].Add(listener, new ActionWrapperWithZeroArgument(listener, action));
            }
        }
        
        public void Subscribe<T, U>(object listener, UnityAction<U> action)
        {
            var type = typeof(T);
            
            if (AddSubscriber<T>(listener))
            {
                _events[type].Add(listener, new ActionWrapperWithOneArgument<U>(listener, action));
            }
        }

        public void Unsubscribe<T>(object listener)
        {
            if (_events is null || _events.Count == 0)
            {
                return;
            }
            
            var type = typeof(T);

            if (_events.TryGetValue(type, out var @event))
            {
                @event.Remove(listener);
            }
        }

        public void Trigger<T>()
        {
            if (_events is null || _events.Count == 0)
            {
                return;
            }
            
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            foreach (var wrapper in events.Select(actionWrapper => actionWrapper.Value))
            {
                var convertedWrapper = wrapper as ActionWrapperWithZeroArgument;
                convertedWrapper?.Invoke();
            }
        }

        public void Trigger<T, U>(U arg)
        {
            if (_events is null || _events.Count == 0)
            {
                return;
            }
            
            var type = typeof(T);

            if (!_events.TryGetValue(type, out var events))
            {
                return;
            }

            foreach (var wrapper in 
                     events.Select(actionWrapper => actionWrapper.Value))
            {
                var convertedWrapper = wrapper as ActionWrapperWithOneArgument<U>;
                convertedWrapper?.Invoke(arg);
            }
        }

        private bool AddSubscriber<T>(object listener)
        {
            _events ??= new Dictionary<Type, Dictionary<object, IActionWrapper>>();
            
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
        
        private void Dispose()
        {
            _events.Clear();
        }
    }
}