using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using MVVMAqua.Helpers;

namespace MVVMAqua.Validation
{
	public class ValidatableProperty<T, TResult> : NotifyObject, IValidatable<T, TResult>
		where TResult : IValidationResult
	{
		private Action OnValueChanged { get; }


		public ValidatableProperty() 
			: this(default, null, false) { }

		public ValidatableProperty(T initialValue) 
			: this(initialValue, null, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged) 
			: this (initialValue, onValueChanged, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged, bool isValidateWhenPropertyChange)
		{
			Value = initialValue;

			OnValueChanged += onValueChanged;
			if (isValidateWhenPropertyChange)
			{
				OnValueChanged += () => Validate(ValidationRules.Where(x => x.IsValidateWhenPropertyChange));
			}
		}


		private protected List<ValidationRuleWrapper<T, TResult>> ValidationRules { get; } = new List<ValidationRuleWrapper<T, TResult>>();
		public ObservableCollection<TResult> Errors { get; } = new ObservableCollection<TResult>();

		private T _value;
		public T Value
		{
			get => _value;
			set => SetProperty(ref _value, value, OnValueChanged, nameof(Value));
		}

		public bool IsValid => Errors.Count == 0;

		public event Action IsValidChanged;


		#region AddRule

		public void AddRule(IValidationRule<T, TResult> rule)
		{
			AddRule(rule, true);
		}

		public void AddRule(IValidationRule<T, TResult> rule, bool isValidateWhenPropertyChange)
		{
			ValidationRules.Add(new ValidationRuleWrapper<T, TResult>(rule.Check, isValidateWhenPropertyChange));
		}

		public void AddRule(Func<T, TResult> rule)
		{
			AddRule(rule, true);
		}

		public void AddRule(Func<T, TResult> rule, bool isValidateWhenPropertyChange)
		{
			if (rule == null)
				throw new ArgumentNullException(nameof(rule));

			ValidationRules.Add(new ValidationRuleWrapper<T, TResult>(rule, isValidateWhenPropertyChange));
		}

		#endregion


		#region Validate

		public bool Validate()
		{
			return Validate(ValidationRules);
		}

		private bool Validate(IEnumerable<ValidationRuleWrapper<T, TResult>> validationRules)
		{
			var isValidPrevious = IsValid;

			Errors.Clear();
			validationRules.ForEach(rule =>
			{
				var result = rule.ValidationRule(Value);
				if (!result.IsValid)
				{
					Errors.Add(result);
				}
			});

			if (isValidPrevious != IsValid)
			{
				OnPropertyChanged(nameof(IsValid));
				IsValidChanged?.Invoke();
			}

			return IsValid;
		}

		#endregion
	}
}