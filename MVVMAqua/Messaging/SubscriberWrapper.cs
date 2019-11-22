using System;

namespace MVVMAqua.Messaging
{
	class SubscriberWrapper
	{
		public WeakReference Subscriber { get; }

		public bool IsAlive => Subscriber.IsAlive;

		public SubscriberWrapper(object subscriber)
		{
			if (subscriber == null)
				throw new ArgumentNullException("subscriber", "Subscriber cannot be null");

			Subscriber = new WeakReference(subscriber);
		}
	}
}