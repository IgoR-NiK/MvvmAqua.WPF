using System;

namespace MVVMAqua.Messaging
{
	class SubscriberWrapper
	{		

		public WeakReference Subscriber { get; }

		public bool IsAlive => Subscriber.IsAlive;

		public SubscriberWrapper(object subscriber)
		{
			Subscriber = new WeakReference(subscriber);
		}
	}
}