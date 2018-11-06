using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMAqua.Validation
{
	internal class ValidationRuleWrapper<T>
	{
		public Guid Id { get; }
		public Func<T, ValidationResult> ValidationRule { get; }

		public ValidationRuleWrapper(Func<T, ValidationResult> validationRule)
		{
			Id = Guid.NewGuid();
			ValidationRule = validationRule;
		}
	}
}