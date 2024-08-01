using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Infrastructure.Service.SignalBus
{
    public class EventSignalBus : ISignalBus
    {
        private Dictionary<Type, Dictionary<object, IActionWrapper>> _events = new();

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
                var convertedWrapper = wrapper as ActionWrapperWithZeroArgument;
                convertedWrapper?.Invoke();
            }
        }

        public void Trigger<T, U>(U arg)
        {
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