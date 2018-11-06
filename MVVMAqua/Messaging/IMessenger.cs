using System;

namespace MVVMAqua.Messaging
{
	public interface IMessenger
	{
		void Send<TMessage>(object sender, TMessage message);
		void Subscribe<TMessage>(object subscriber, Action<object, TMessage> action);
		void Unsubscribe<TMessage>(object subscriber);

		void ClearSubscribes();
		void ClearSubscribes<TMessage>();
	}
}