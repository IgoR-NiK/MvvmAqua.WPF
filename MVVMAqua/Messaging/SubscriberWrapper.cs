using System;

namespace MVVMAqua.Messaging
{
	internal class SubscriberWrapper
	{
		public WeakReference Subscriber { get; }

		public bool IsAlive => Subscriber.IsAlive;

		public SubscriberWrapper(object subscriber)
		{
			if (subscriber == null) 
				throw new ArgumentNullException(nameof(subscriber));

			Subscriber = new WeakReference(subscriber);
		}
	}
}