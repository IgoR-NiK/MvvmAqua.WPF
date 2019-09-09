using System;

namespace MVVMAqua.Validation
{
	public class ValidationResult : IValidationResult
	{
		public static ValidationResult Ok { get; } = new ValidationResult();
		

		public bool IsValid { get; }

		public string Message { get; }


		public ValidationResult() : this(true, String.Empty) { }

		public ValidationResult(bool isValid) : this(isValid, String.Empty) { }

		public ValidationResult(bool isValid, string message)
		{
			IsValid = isValid;
			Message = message;
		}


		public override string ToString() => Message;
	}
}