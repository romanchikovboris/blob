using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EventBusSystem
{
    /// <summary>
    /// Global (interface based) event bus
    /// </summary>
    public static class EventBus
    {
        private static Dictionary<Type, SubscribersList<IGlobalSubscriber>> s_Subscribers
            = new Dictionary<Type, SubscribersList<IGlobalSubscriber>>();
        
        public static void Subscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            foreach (Type t in subscriberTypes)
            {
                if (!s_Subscribers.ContainsKey(t))
                {
                    s_Subscribers[t] = new SubscribersList<IGlobalSubscriber>();
                }
                s_Subscribers[t].Add(subscriber);
            }
        }
        
        public static void Unsubscribe(IGlobalSubscriber subscriber)
        {
            List<Type> subscriberTypes = EventBusHelper.GetSubscriberTypes(subscriber);
            foreach (Type t in subscriberTypes)
            {
                if (s_Subscribers.ContainsKey(t))
                    s_Subscribers[t].Remove(subscriber);
            }
        }
        
        public static void RaiseEvent<TSubscriber>(Action<TSubscriber> action)
            where TSubscriber : class, IGlobalSubscriber
        {
            if(!s_Subscribers.ContainsKey(typeof(TSubscriber)))
            {
#if UNITY_EDITOR
                Debug.LogWarning("Attempt to raise event without subscribers.Is it expected behaviour?");
#endif
                return;
            }
            
            SubscribersList<IGlobalSubscriber> subscribers = s_Subscribers[typeof(TSubscriber)];
	
            subscribers.Executing = true;
            foreach (IGlobalSubscriber subscriber in subscribers.List.ToList())
            {
                try
                {
                    action.Invoke(subscriber as TSubscriber);
                }
                catch (Exception e)
                {
                    Debug.LogError(e);
                }
            }
            subscribers.Executing = false;
            subscribers.Cleanup();
        }
    }
}