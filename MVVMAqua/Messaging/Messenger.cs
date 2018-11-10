using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Messaging
{
	public sealed class Messenger : IMessenger
	{
		Dictionary<Type, List<SubscriberWrapper>> Subscribers { get; } = new Dictionary<Type, List<SubscriberWrapper>>();

		public void Send<TMessage>(object sender, TMessage message)
		{
			Subscribers[typeof(TMessage)]?.ForEach(s => s.Action(sender, message));
			RefreshSubscribers();
		}

		public void Subscribe<TMessage>(object subscriber, Action<object, TMessage> action)
		{
			if (!Subscribers.ContainsKey(typeof(TMessage)))
			{
				Subscribers.Add(typeof(TMessage), new List<SubscriberWrapper>());
			}

			var wrap = new SubscriberWrapper(subscriber);
			wrap.SetAction(action);
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
				if (pair.Value.Count == 0)
				{
					deletedKeys.Add(pair.Key);
				}
			}

			deletedKeys.ForEach(key => Subscribers.Remove(key));
		}
	}
}