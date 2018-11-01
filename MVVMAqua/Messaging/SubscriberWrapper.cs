using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MvvmAqua.Messaging
{
	class SubscriberWrapper
	{
		public WeakReference Subscriber { get; }

		private object sender;
		private object message;
		private Action action;

		public SubscriberWrapper(object subscriber)
		{
			Subscriber = new WeakReference(subscriber);
		}

		public void SetAction<TMessage>(Action<object, TMessage> action)
		{
			this.action = () => action(sender, (TMessage)message);
		}

		public void Action<TMessage>(object sender, TMessage message)
		{
			this.sender = sender;
			this.message = message;
			action();
		}

		public bool IsAlive => Subscriber.IsAlive;
	}
}