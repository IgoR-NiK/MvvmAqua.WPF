using System;

namespace MVVMAqua.Validation
{
	public class ValidatableProperty<T> : ValidatableProperty<T, ValidationResult>
	{
		public ValidatableProperty() 
			: this(default, null, false) { }

		public ValidatableProperty(T initialValue) 
			: this(initialValue, null, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged) 
			: this (initialValue, onValueChanged, false) { }

		public ValidatableProperty(T initialValue, Action onValueChanged, bool isValidateWhenPropertyChange) 
			: base(initialValue, onValueChanged, isValidateWhenPropertyChange) { }


		#region AddRule

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

			ValidationRules.Add(new ValidationRuleWrapper<T, ValidationResult>(x => new ValidationResult(rule(x), message), isValidateWhenPropertyChange));
		}

		#endregion
	}
}