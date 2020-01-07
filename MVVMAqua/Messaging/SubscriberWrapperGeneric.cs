using System;

namespace MVVMAqua.Messaging
{
	internal class SubscriberWrapper<TMessage> : SubscriberWrapper
	{
		public Action<object, TMessage> Action { get; }

		public SubscriberWrapper(object subscriber, Action<object, TMessage> action)
			: base(subscriber)
		{
			Action = action ?? throw new ArgumentNullException(nameof(action));
		}
	}
}