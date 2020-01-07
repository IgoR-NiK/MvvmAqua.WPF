using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMAqua.Messaging
{
	public sealed class Messenger : IMessenger
	{
		private Dictionary<Type, List<SubscriberWrapper>> Subscribers { get; } = new Dictionary<Type, List<SubscriberWrapper>>();

		public void Send<TMessage>(object sender, TMessage message)
		{
			Subscribers[typeof(TMessage)]?.ForEach(s => (s as SubscriberWrapper<TMessage>)?.Action(sender, message));
			RefreshSubscribers();
		}

		public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> action)
		{
			if (!Subscribers.ContainsKey(typeof(TMessage)))
			{
				Subscribers.Add(typeof(TMessage), new List<SubscriberWrapper>());
			}

			var wrap = new SubscriberWrapper<TMessage>(subscriber, action);
			Subscribers[typeof(TMessage)].Add(wrap);

			RefreshSubscribers();
		}

		public void Unsubscribe<TMessage>(object subscriber)
		{
			Subscribers[typeof(TMessage)]?.RemoveAll(s => s.Subscriber.Target == subscriber);
			RefreshSubscribers();
		}
		
		public void ClearSubscribes()
		{
			Subscribers.Clear();
		}

		public void ClearSubscribes<TMessage>()
		{			
			Subscribers.Remove(typeof(TMessage));
			RefreshSubscribers();
		}

		private void RefreshSubscribers()
		{
			var deletedKeys = new List<Type>();

			foreach (var pair in Subscribers)
			{
				pair.Value.RemoveAll(s => !s.IsAlive);
				if (!pair.Value.Any())
				{
					deletedKeys.Add(pair.Key);
				}
			}

			deletedKeys.ForEach(key => Subscribers.Remove(key));
		}
	}
}