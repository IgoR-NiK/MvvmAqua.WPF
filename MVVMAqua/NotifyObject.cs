using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq.Expressions;

namespace MVVMAqua
{
	public abstract class NotifyObject : INotifyPropertyChanged
	{
		protected void SetProperty<T>(ref T property, T value, string propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				OnPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Expression<Func<T>> propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				OnPropertyChanged(propertyName);
			}
		}

		protected void SetProperty<T>(ref T property, T value, Action onValueChanged, string propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				onValueChanged?.Invoke();
				OnPropertyChanged(propertyName);
			}
		}
		
		protected void SetProperty<T>(ref T property, T value, Action onValueChanged, Expression<Func<T>> propertyName)
		{
			if (!EqualityComparer<T>.Default.Equals(property, value))
			{
				property = value;
				onValueChanged?.Invoke();
				OnPropertyChanged(propertyName);
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void OnPropertyChanged<T>(Expression<Func<T>> property)
		{
			if (PropertyChanged != null)
			{
				if (property.Body is MemberExpression expression)
				{
					PropertyChanged(this, new PropertyChangedEventArgs(expression.Member.Name));
				}
			}
		}
	}
}