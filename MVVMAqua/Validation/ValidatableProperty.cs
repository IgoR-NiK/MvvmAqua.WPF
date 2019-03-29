using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Specialized;
using MVVMAqua.Helpers;

namespace MVVMAqua.Validation
{
	public class ValidatableProperty<T> : NotifyObject, IValidity
	{
		private Action OnValueChanged { get; }


		public ValidatableProperty() : this(default, null, false) { }

		public ValidatableProperty(T initialValue) : this(initialValue, null, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged) : this (initialValue, onValueChanged, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged, bool isValidateWhenPropertyChange)
		{
			Value = initialValue;

			OnValueChanged += onValueChanged;
			if (isValidateWhenPropertyChange)
			{
				OnValueChanged += () => Validate(ValidationRules.Where(x => x.IsValidateWhenPropertyChange));
			}
		}


		private List<ValidationRuleWrapper<T>> ValidationRules { get; } = new List<ValidationRuleWrapper<T>>();
		public ObservableCollection<ValidationResult> Errors { get; } = new ObservableCollection<ValidationResult>();

		private T _value;
		public T Value
		{
			get => _value;
			set => SetProperty(ref _value, value, OnValueChanged, "Value");
		}

		public bool IsValid => Errors.Count == 0;

		public event Action IsValidChanged;


		#region AddRule

		public void AddRule(IValidationRule<T> rule)
		{
			AddRule(rule, true);
		}

		public void AddRule(IValidationRule<T> rule, bool isValidateWhenPropertyChange)
		{
			ValidationRules.Add(new ValidationRuleWrapper<T>(rule.Check, isValidateWhenPropertyChange));
		}

		public void AddRule(Func<T, ValidationResult> rule)
		{
			AddRule(rule, true);
		}

		public void AddRule(Func<T, ValidationResult> rule, bool isValidateWhenPropertyChange)
		{
			if (rule == null)
			{
				throw new ArgumentException("Не указано правило валидации.");
			}

			ValidationRules.Add(new ValidationRuleWrapper<T>(rule, isValidateWhenPropertyChange));
		}

		public void AddRule(Func<T, bool> rule, string message)
		{
			AddRule(rule, message, true);
		}

		public void AddRule(Func<T, bool> rule, string message, bool isValidateWhenPropertyChange)
		{
			if (rule == null)
			{
				throw new ArgumentException("Не указано правило валидации.");
			}

			ValidationRules.Add(new ValidationRuleWrapper<T>(x => new ValidationResult(rule(x), message), isValidateWhenPropertyChange));
		}

		#endregion


		#region Validate

		public bool Validate()
		{
			return Validate(ValidationRules);
		}

		private bool Validate(IEnumerable<ValidationRuleWrapper<T>> validationRules)
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