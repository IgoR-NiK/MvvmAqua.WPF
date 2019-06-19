using System;

namespace MVVMAqua.Validation
{
	internal class ValidationRuleWrapper<T, TResult>
		where TResult : IValidationResult
	{
		public Func<T, TResult> ValidationRule { get; }

		public bool IsValidateWhenPropertyChange { get; }


		public ValidationRuleWrapper(Func<T, TResult> validationRule, bool isValidateWhenPropertyChange)
		{
			ValidationRule = validationRule;
			IsValidateWhenPropertyChange = isValidateWhenPropertyChange;
		}
	}
}