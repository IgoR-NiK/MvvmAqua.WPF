using System;

namespace MVVMAqua.Messaging
{
	class SubscriberWrapper
	{
		public WeakReference Subscriber { get; protected set; }

		public bool IsAlive => Subscriber.IsAlive;
	}
}