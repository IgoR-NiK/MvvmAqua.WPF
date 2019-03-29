using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Validation
{
	internal class ValidationRuleWrapper<T>
	{
		public Func<T, ValidationResult> ValidationRule { get; }

		public bool IsValidateWhenPropertyChange { get; }


		public ValidationRuleWrapper(Func<T, ValidationResult> validationRule, bool isValidateWhenPropertyChange)
		{
			ValidationRule = validationRule;
			IsValidateWhenPropertyChange = isValidateWhenPropertyChange;
		}
	}
}