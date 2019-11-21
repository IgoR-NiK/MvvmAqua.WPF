using System;

namespace MVVMAqua.Messaging
{
    class SubscriberWrapper<TMessage> : SubscriberWrapper
    {		
        public Action<object, TMessage> Action { get; }
						
        public SubscriberWrapper(object subscriber, Action<object, TMessage> action)
        {
            Subscriber = new WeakReference(subscriber);
            Action = action;
        }
    }
}